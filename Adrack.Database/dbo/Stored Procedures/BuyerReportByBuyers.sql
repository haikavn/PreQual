-- =============================================
-- Company: Adrack.com
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Buyer Report By Buyer Channels
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[BuyerReportByBuyerChannels]
-- =============================================

CREATE PROCEDURE [dbo].[BuyerReportByBuyers]
    @Start         DATETIME,
	@End           DATETIME,
	@Buyers        VARCHAR(MAX),
	@AffiliateChannels VARCHAR(MAX),
	@Campaigns     VARCHAR(MAX)
AS
BEGIN
-- =================================================================================================
-- SQL Script Setup
-- =================================================================================================
	SET NOCOUNT ON

	declare @TimeZone int = 0;

	select @TimeZone = cast([Value] as int) from Setting where [Key] = 'TimeZone';

	if @TimeZone is null 
	begin
		set @TimeZone = 0;
	end;

-- =================================================================================================
-- SQL Common
-- =================================================================================================
	DECLARE @SQLError_Severity INT
	DECLARE @SQLError_State    INT
	DECLARE @SQLError_Message  NVARCHAR(2000)

-- =================================================================================================
-- Drop SQL Temp Tables
-- =================================================================================================
	IF OBJECT_ID('[TempDB].[dbo].[#BuyersId]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#BuyersId]
		END

	IF OBJECT_ID('[TempDB].[dbo].[#AffiliateChannelsId]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#AffiliateChannelsId]
		END

	IF OBJECT_ID('[TempDB].[dbo].[#CampaignsId]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#CampaignsId]
		END

-- =================================================================================================
-- SQL Temp Tables
-- =================================================================================================
	CREATE TABLE [dbo].[#BuyersId]
	(	
		[Id] BIGINT NULL,
	)

	CREATE TABLE [dbo].[#AffiliateChannelsId]
	(	
		[Id] BIGINT NULL,
	)

	CREATE TABLE [dbo].[#CampaignsId]
	(	
		[Id] BIGINT NULL,
	)

	print @start;
	print @end;

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY	
		--------- Insert Parsed Buyers Id ---------
		INSERT INTO [dbo].[#BuyersId] 
		SELECT [Item] 
		FROM [dbo].SplitInts(@Buyers, ',');

		--------- Insert Parsed Affiliates Channel Id ---------
		INSERT INTO [dbo].[#AffiliateChannelsId] 
		SELECT [Item] 
		FROM [dbo].SplitInts(@AffiliateChannels, ',');

		--------- Insert Parsed Campaign Id ---------
		INSERT INTO [dbo].[#CampaignsId] 
		SELECT [Item] 
		FROM [dbo].SplitInts(@Campaigns, ',');

		--------- Report ---------
		SELECT b.Id AS BuyerId
			,b.NAME AS BuyerName
			,ISNULL(SUM(lr.Quantity), 0) AS TotalLeads
			,(
				SELECT ISNULL(SUM(Quantity), 0)
				FROM LeadMainReportDay
				WHERE BuyerId = lr.BuyerId
					AND STATUS = 1
					AND Created = lr.Created
					AND Created BETWEEN @Start
						AND @End
				) AS SoldLeads
			,(
				SELECT ISNULL(SUM(Quantity), 0)
				FROM LeadMainReportDay
				WHERE BuyerId = lr.BuyerId
					AND STATUS = 3
					AND Created = lr.Created
					AND Created BETWEEN @Start
						AND @End
				) AS RejectedLeads
			,ISNULL((
					SELECT ISNULL(SUM(BuyerPrice), 0)
					FROM LeadMainReportDay
					WHERE BuyerId = lr.BuyerId
						AND STATUS = 1
						AND Created = lr.Created
						AND Created BETWEEN @Start
							AND @End
					), 0) AS Cost
			,ISNULL((
					SELECT ISNULL(SUM(quantity), 0)
					FROM LeadMainReportDayRedirected
					WHERE BuyerId = lr.BuyerId
						AND Created = lr.Created
						AND Created BETWEEN @Start
							AND @End
					), 0) AS Redirected
			,(
				SELECT ISNULL(COUNT(lr.LeadId), 0)
				FROM LeadMainReport lr
				INNER JOIN LeadNote n ON n.LeadId = lr.LeadId
				WHERE dateadd(hour, @TimeZone, lr.created) BETWEEN @Start
						AND @End
				) AS loanedleads,
				(select top 1 LastPostedSold from Buyer where id = lr.BuyerId) as LastSoldDate
		FROM LeadMainReportDay lr
		INNER JOIN Buyer b ON b.Id = lr.BuyerId
		WHERE lr.BuyerId IN (
				SELECT Id
				FROM [dbo].[#BuyersId]
				)
			AND lr.AffiliateChannelId IN (
				SELECT Id
				FROM [dbo].[#AffiliateChannelsId]
				)
			AND lr.CampaignId IN (
				SELECT Id
				FROM [dbo].[#CampaignsId]
				)
			AND lr.Created BETWEEN @Start
				AND @End
			AND lr.STATUS != 7
			AND lr.STATUS != 5
		GROUP BY b.Id
			,b.NAME
			,lr.BuyerId
			,lr.BuyerChannelId
			,lr.CampaignId
			,lr.created
		ORDER BY b.name
	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END