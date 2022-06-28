-- =============================================
-- Company: Adrack.com
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Buyer Report By Buyer Channels
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[BuyerReportByBuyerChannels]
-- =============================================

CREATE PROCEDURE [dbo].[ReportBuyersByPrices]
    @Start         date,
	@End           date,
	@Buyers        VARCHAR(MAX),
	@BuyerChannels VARCHAR(MAX),
	@Campaigns     VARCHAR(MAX),
	@FromPrice		money,
	@ToPrice		money
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

	IF OBJECT_ID('[TempDB].[dbo].[#CampaignsId]', 'U') IS NOT NULL
		BEGIN
			DROP TABLE [dbo].[#CampaignsId]
		END

-- =================================================================================================
-- SQL Temp Tables
-- =================================================================================================
	CREATE TABLE [dbo].[#BuyersId]
	(	
		[Id] BIGINT NULL
	)

	CREATE TABLE [dbo].[#BuyerChannelsId]
	(	
		[Id] BIGINT NULL
	)

	CREATE TABLE [dbo].[#CampaignsId]
	(	
		[Id] BIGINT NULL
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

		INSERT INTO [dbo].[#BuyerChannelsId] 
		SELECT [Item] 
		FROM [dbo].SplitInts(@BuyerChannels, ',');

		--------- Insert Parsed Campaign Id ---------
		INSERT INTO [dbo].[#CampaignsId] 
		SELECT [Item] 
		FROM [dbo].SplitInts(@Campaigns, ',');

		SELECT b.Id AS BuyerChannelId
				,b.NAME AS BuyerChannelName
				,lr.BuyerPrice
				,lr.[Status]
				,sum(lr.Quantity) as Quantity
				,sum(lr.UQuantity) as UQuantity
		FROM LeadMainReportDayPrices lr
		INNER JOIN BuyerChannel b ON b.Id = lr.BuyerChannelId
		WHERE lr.BuyerChannelId IN (
				SELECT Id
				FROM [dbo].[#BuyerChannelsId]
				)
			AND lr.CampaignId IN (
				SELECT Id
				FROM [dbo].[#CampaignsId]
				)
			AND lr.Created BETWEEN @Start AND @End
			AND (lr.BuyerPrice >= @FromPrice and lr.BuyerPrice <= @ToPrice)

		GROUP BY b.Id
			,b.NAME
			,lr.BuyerId
			,lr.[Status]
			,lr.BuyerPrice
		ORDER BY b.name
	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END