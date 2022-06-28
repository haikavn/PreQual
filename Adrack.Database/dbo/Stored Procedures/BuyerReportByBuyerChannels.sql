-- =============================================
-- Company: Adrack.com
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Buyer Report By Buyer Channels
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[BuyerReportByBuyerChannels]
-- =============================================

CREATE PROCEDURE [dbo].[BuyerReportByBuyerChannels]
    @Start         DATETIME,
	@End           DATETIME,
	@Buyers        VARCHAR(MAX),
	@BuyerChannels VARCHAR(MAX),
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

	IF OBJECT_ID('[TempDB].[dbo].[#BuyerChannelsId]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#BuyerChannelsId]
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

	CREATE TABLE [dbo].[#BuyerChannelsId]
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

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY	
		--------- Insert Parsed Buyers Id ---------
		INSERT INTO [dbo].[#BuyersId] 
		SELECT [Item] 
		FROM [dbo].SplitInts(@Buyers, ',');

		--------- Insert Parsed Buyers Channel Id ---------
		INSERT INTO [dbo].[#BuyerChannelsId] 
		SELECT [Item] 
		FROM [dbo].SplitInts(@BuyerChannels, ',');

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
			,b.Name AS BuyerName
			,bc.Id AS BuyerChannelId
			,bc.Name AS BuyerChannelName
			,ISNULL(SUM(lr.Quantity), 0) AS TotalLeads
			,(
				SELECT ISNULL(SUM(Quantity), 0)
				FROM LeadMainReportDay
				WHERE BuyerId = lr.BuyerId
					AND BuyerChannelId = lr.BuyerChannelId
					AND Status = 1
					AND Created = lr.Created
					AND Created BETWEEN @Start
						AND @End
				) AS SoldLeads
				,(
				SELECT ISNULL(SUM(Quantity), 0)
				FROM LeadMainReportDay
				WHERE BuyerId = lr.BuyerId
					AND BuyerChannelId = lr.BuyerChannelId
					AND STATUS = 3
					AND Created = lr.Created
					AND Created BETWEEN @Start
						AND @End
				) AS RejectedLeads
			,ISNULL((
					SELECT ISNULL(SUM(BuyerPrice), 0)
					FROM LeadMainReportDay
					WHERE BuyerId = lr.BuyerId
						AND BuyerChannelId = lr.BuyerChannelId
						AND Status = 1
						AND Created = lr.Created
						AND Created BETWEEN @Start
							AND @End
					), 0) AS Cost
			,ISNULL((
					SELECT ISNULL(SUM(AffiliatePrice), 0)
					FROM LeadMainReportDay
					WHERE BuyerId = lr.BuyerId
						AND BuyerChannelId = lr.BuyerChannelId
						AND Status = 1
						AND Created = lr.Created
						AND Created BETWEEN @Start
							AND @End
					), 0) AS AffiliatePrice
			,ISNULL((
					SELECT ISNULL(AVG(BuyerPrice), 0)
					FROM LeadMainReportDay
					WHERE BuyerId = lr.BuyerId
						AND BuyerChannelId = lr.BuyerChannelId
						AND Status = 1
						AND Created = lr.Created
						AND Created BETWEEN @Start
							AND @End
					), 0) AS AveragePrice
					,bc.OrderNum
			,(
				SELECT ISNULL(COUNT(lr.LeadId), 0)
				FROM LeadMainReport lr
				INNER JOIN LeadNote n ON n.LeadId = lr.LeadId
				WHERE dateadd(hour, @TimeZone, lr.Created) BETWEEN @Start
						AND @End
				) AS loanedleads,
				(SELECT        ISNULL(sum(quantity), 0) AS Expr1
				FROM            dbo.LeadMainReportDayRedirected where campaigntype = 0 and created = cast(lr.Created as date)  and created between @Start and @End AND BuyerChannelId = lr.BuyerChannelId
				)  AS redirected,	
				(select top 1 LastPostedSold from Buyer where id = lr.BuyerId) as LastSoldDate
		FROM LeadMainReportDay lr
		INNER JOIN Buyer b ON b.Id = lr.BuyerId
		INNER JOIN BuyerChannel bc ON bc.Id = lr.BuyerChannelId
		WHERE lr.BuyerId IN (
				SELECT Id
				FROM [dbo].[#BuyersId]
				)
			AND lr.BuyerChannelId IN (
				SELECT Id
				FROM [dbo].[#BuyerChannelsId]
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
			AND lr.Status != 7
			AND lr.Status != 5
		GROUP BY b.Id
			,b.Name
			,bc.Id
			,bc.Name
			,bc.OrderNum
			,lr.BuyerId
			,lr.BuyerChannelId
			,lr.CampaignId
			,lr.Created
		ORDER BY b.Name, bc.Name
	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END