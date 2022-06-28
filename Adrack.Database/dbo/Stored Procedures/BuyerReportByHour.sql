-- =============================================
-- Company: Adrack.com
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Buyer Report By Buyer Channels
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[BuyerReportByBuyerChannels]
-- =============================================

CREATE PROCEDURE [dbo].[BuyerReportByHour]
    @date         date,
	@BuyerChannels VARCHAR(MAX),
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

	IF OBJECT_ID('[TempDB].[dbo].[#BuyerChannelsId]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#BuyerChannelsId]
		END

	IF OBJECT_ID('[TempDB].[dbo].[#CampaignsId]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#CampaignsId]
		END

-- =================================================================================================
-- SQL Temp Tables
-- =================================================================================================

	CREATE TABLE [dbo].[#BuyerChannelsId]
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
		--------- Insert Parsed Buyers Channel Id ---------
		INSERT INTO [dbo].[#BuyerChannelsId] 
		SELECT [Item] 
		FROM [dbo].SplitInts(@BuyerChannels, ',');


		--------- Insert Parsed Campaign Id ---------
		INSERT INTO [dbo].[#CampaignsId] 
		SELECT [Item] 
		FROM [dbo].SplitInts(@Campaigns, ',');

		--------- Report ---------
		SELECT b.Id AS BuyerId
			,b.Name AS BuyerName
			,bc.Id AS BuyerChannelId
			,bc.Name AS BuyerChannelName
			,lr.[Hour]
			,ISNULL(SUM(lr.Quantity), 0) AS TotalLeads
			,(
				SELECT ISNULL(SUM(Quantity), 0)
				FROM LeadMainReportDayHour
				WHERE BuyerId = lr.BuyerId
					AND BuyerChannelId = lr.BuyerChannelId
					AND Status = 1
					AND Created = lr.Created
					AND [Hour] = lr.[Hour]
					AND Created = @date
				) AS SoldLeads
			,ISNULL((
					SELECT ISNULL(SUM(BuyerPrice), 0)
					FROM LeadMainReportDayHour
					WHERE BuyerId = lr.BuyerId
						AND BuyerChannelId = lr.BuyerChannelId
						AND Status = 1
						AND Created = lr.Created
						AND [Hour] = lr.[Hour]
						AND Created = @date
					), 0) AS Cost
			
		FROM LeadMainReportDayHour lr
		INNER JOIN Buyer b ON b.Id = lr.BuyerId
		INNER JOIN BuyerChannel bc ON bc.Id = lr.BuyerChannelId
		WHERE  lr.BuyerChannelId IN (
				SELECT Id
				FROM [dbo].[#BuyerChannelsId]
				)			
			AND lr.CampaignId IN (
				SELECT Id
				FROM [dbo].[#CampaignsId]
				)
			AND lr.Created = @date
			AND lr.Status != 7
			AND lr.Status != 5
		GROUP BY b.Id
			,b.Name
			,bc.Id
			,bc.Name
			,lr.BuyerId
			,lr.BuyerChannelId
			,lr.CampaignId
			,lr.Created
			,lr.[Hour]
		ORDER BY b.Name, bc.Name
	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END