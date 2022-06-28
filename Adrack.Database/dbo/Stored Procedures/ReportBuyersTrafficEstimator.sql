-- =============================================
-- Company: Adrack.com
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Buyer Report By Buyer Channels
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[BuyerReportByBuyerChannels]
-- =============================================

CREATE PROCEDURE [dbo].[ReportBuyersTrafficEstimator]
    @sqlQuery varchar(MAX)
AS
BEGIN
-- =================================================================================================
-- SQL Script Setup
-- =================================================================================================
	SET NOCOUNT ON

	exec(@sqlQuery);

--	declare @TimeZone int = 0;

--	--select @TimeZone = cast([Value] as int) from Setting where [Key] = 'TimeZone';

--	if @TimeZone is null 
--	begin
--		set @TimeZone = 0;
--	end;

---- =================================================================================================
---- SQL Common
---- =================================================================================================
--	DECLARE @SQLError_Severity INT
--	DECLARE @SQLError_State    INT
--	DECLARE @SQLError_Message  NVARCHAR(2000)

---- =================================================================================================
---- Drop SQL Temp Tables
---- =================================================================================================

--	IF OBJECT_ID('[TempDB].[dbo].[#BuyerChannelsId]', 'U') IS NOT NULL
--		BEGIN
--			DROP TABLE [dbo].[#BuyerChannelsId]
--		END

--	IF OBJECT_ID('[TempDB].[dbo].[#CampaignsId]', 'U') IS NOT NULL
--		BEGIN
--			DROP TABLE [dbo].[#CampaignsId]
--		END

---- =================================================================================================
---- SQL Temp Tables
---- =================================================================================================
--	CREATE TABLE [dbo].[#BuyerChannelsId]
--	(	
--		[Id] BIGINT NULL
--	)

--	CREATE TABLE [dbo].[#CampaignsId]
--	(	
--		[Id] BIGINT NULL
--	)

---- =================================================================================================
---- Process Script
---- =================================================================================================
-- 	BEGIN TRY	
--		INSERT INTO [dbo].[#BuyerChannelsId] 
--		SELECT [Item] 
--		FROM [dbo].SplitInts(@BuyerChannels, ',');

		--------- Insert Parsed Campaign Id ---------
	--	INSERT INTO [dbo].[#CampaignsId] 
	--	SELECT [Item] 
	--	FROM [dbo].SplitInts(@Campaigns, ',');

	--	declare @IntValue1 int; 
	--	declare @IntValue2 int;
	--	declare @DecimalValue1 decimal;
	--	declare @DecimalValue2 decimal; 
	--	declare @DateValue1 date;
	--	declare @DateValue2 date;

	--	if @ValueType = 2 or @ValueType = 4
	--	begin
	--		set @IntValue1 = cast(@Value1 as int);
	--		set @IntValue2 = cast(@Value2 as int);
	--	end
	--	else
	--	if @ValueType = 16
	--	begin
	--		set @DecimalValue1 = cast(@Value1 as decimal);
	--		set @DecimalValue2 = cast(@Value2 as decimal);
	--	end;

	--	print @IntValue1;
	--	print @IntValue2;

	--	--------- Report ---------
	--	SELECT   Buyer.Id AS BuyerId
	--			,Buyer.Name AS BuyerName
	--			,BuyerChannel.Id AS BuyerChannelId
	--			,BuyerChannel.Name AS BuyerChannelName
	--			,cast(lr.Created as Date) as Created
	--			,count(lr.leadid) as Quantity
	--			,count(distinct lr.leadid) as UQuantity
	--	FROM LeadMainResponse lr
	--	INNER JOIN Buyer ON Buyer.Id = lr.BuyerId
	--	INNER JOIN BuyerChannel ON BuyerChannel.Id = lr.BuyerChannelId
	--	INNER JOIN LeadFieldsContent lc on lc.LeadId = lr.LeadId and lc.FieldName = @Field
	--			AND 
	--			(
	--				(@ValueType = 0 AND StringValue = @Value1) or
	--				(@ValueType = 1 AND StringValue = @Value1) or
	--				(@ValueType = 2 AND IntValue between @IntValue1 and @IntValue2) or
	--				(@ValueType = 3 AND StringValue = @Value1) or
	--				(@ValueType = 4 AND IntValue between @IntValue1 and @IntValue2) or
	--				(@ValueType = 5 AND StringValue = @Value1) or
	--				(@ValueType = 6 AND StringValue = @Value1) or
	--				(@ValueType = 7 AND StringValue = @Value1) or
	--				(@ValueType = 8 AND StringValue = @Value1) or
	--				(@ValueType = 9 AND StringValue = @Value1) or
	--				(@ValueType = 11 AND StringValue = @Value1) or
	--				(@ValueType = 12 AND StringValue = @Value1) or
	--				(@ValueType = 16 AND DecimalValue between @DecimalValue1 and @DecimalValue2)
	--			)
	--	WHERE 
	--		(len(@BuyerChannels) = 0 or lr.BuyerChannelId IN (
	--			SELECT Id
	--			FROM [dbo].[#BuyerChannelsId]
	--			))
	--		AND (len(@Campaigns) = 0 or lr.CampaignId IN (
	--			SELECT Id
	--			FROM [dbo].[#CampaignsId]
	--			))
	--		AND lr.Created BETWEEN @Start AND @End

	--		AND (
	--			((@Price1 > 0 or @Price2 > 0) and lr.BuyerPrice between @Price1 and @Price2)
	--		)

	--	GROUP BY 
	--		 Buyer.Id
	--		,Buyer.Name
	--		,BuyerChannel.Id
	--		,BuyerChannel.Name
	--		,cast(lr.created as Date)
	--	ORDER BY Buyer.name
	--END TRY

	--BEGIN CATCH
	--	SELECT @SQLError_Severity = ERROR_SEVERITY(), 
	--	       @SQLError_State    = ERROR_STATE(), 
	--		   @SQLError_Message  = ERROR_MESSAGE()

	--	RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	--END CATCH

END