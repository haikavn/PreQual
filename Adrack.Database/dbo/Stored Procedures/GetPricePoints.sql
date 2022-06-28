-- =============================================
-- Company: Adrack.com
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Buyer Report By Buyer Channels
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[BuyerReportByBuyerChannels]
-- =============================================

CREATE PROCEDURE [dbo].[GetPricePoints]
    @Start         DATETIME,
	@End           DATETIME,
	@BuyerChannels VARCHAR(MAX),
	@isnot bit
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


-- =================================================================================================
-- SQL Temp Tables
-- =================================================================================================
	CREATE TABLE [dbo].[#BuyerChannelsId]
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

		select distinct BuyerPrice from LeadMainReportDayPrices lr WHERE 
			((@isnot = 0 and
			lr.BuyerChannelId IN (
				SELECT Id
				FROM [dbo].[#BuyerChannelsId]
				))
				or
			(@isnot = 1 and
			lr.BuyerChannelId NOT IN (
				SELECT Id
				FROM [dbo].[#BuyerChannelsId]
				)))
			AND lr.Created BETWEEN @Start
				AND @End
			AND lr.STATUS != 7
			AND lr.STATUS != 5
			AND lr.BuyerPrice > 0;
	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END