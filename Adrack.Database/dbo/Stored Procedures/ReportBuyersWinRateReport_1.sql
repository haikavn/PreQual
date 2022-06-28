-- =============================================
-- Company: Adrack.com
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Buyer Report By Buyer Channels
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[BuyerReportByBuyerChannels]
-- =============================================

CREATE PROCEDURE [dbo].[ReportBuyersWinRateReport] 
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@buyerChannels varchar(MAX)
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
		[Id] BIGINT NULL
	)
	print @start;
	print @end;

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY	
		INSERT INTO [dbo].[#BuyerChannelsId] 
		SELECT [Item] 
		FROM [dbo].SplitInts(@BuyerChannels, ',');

		SELECT b.Id AS BuyerChannelId
				,b.NAME AS BuyerChannelName
				,lr.BuyerPrice
				,sum(lr.Quantity) as TotalLeads
				,(select ISNULL(sum(Quantity), 0) from LeadMainReportDayPrices where Created BETWEEN @Start AND @End and buyerchannelid=lr.buyerchannelid and BuyerPrice=lr.BuyerPrice and [status] = 1) as SoldLeads
				,(select ISNULL(sum(Quantity), 0) from LeadMainReportDayPrices where Created BETWEEN @Start AND @End and buyerchannelid=lr.buyerchannelid and BuyerPrice=lr.BuyerPrice and [status] = 3) as RejectedLeads
				,(SELECT	ISNULL(count(distinct leadid), 0) AS Expr1
				  FROM dbo.LeadMainResponse where status = 3 and ErrorType = 14 and campaigntype = 0 and buyerchannelid=lr.buyerchannelid and BuyerPrice=lr.BuyerPrice and cast(Created as date) BETWEEN @Start AND @End
								   ) as MinPriceErrorLeads
		FROM LeadMainReportDayPrices lr
		INNER JOIN BuyerChannel b ON b.Id = lr.BuyerChannelId
		WHERE lr.BuyerPrice> 0 and lr.BuyerChannelId IN (
				SELECT Id
				FROM [dbo].[#BuyerChannelsId]
				)
			AND lr.Created BETWEEN @Start AND @End

		GROUP BY b.Id
			,b.NAME
			,lr.BuyerChannelId
			,lr.BuyerPrice
		ORDER BY lr.BuyerPrice
	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END