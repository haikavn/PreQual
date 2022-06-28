CREATE PROCEDURE [dbo].[ReportBuyersByPricesFinal]
	(
    @Start         date,
	@End           date,
	@Buyers        VARCHAR(MAX),
	@BuyerChannels VARCHAR(MAX),
	@Campaigns     VARCHAR(MAX),
	@FromPrice		money,
	@ToPrice		money
)
AS
BEGIN

-- =================================================================================================
-- SQL Common
-- =================================================================================================
	DECLARE @SQLError_Severity INT
	DECLARE @SQLError_State    INT
	DECLARE @SQLError_Message  NVARCHAR(2000)

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY

    DECLARE @buyerByPricesReport TABLE (BuyerChannelId bigint not null, BuyerChannelName varchar(50),
										BuyerPrice money, [Status] smallint, Quantity int, UQuantity int)

    INSERT @buyerByPricesReport EXEC dbo.ReportBuyersByPrices @Start, @End,  @Buyers, @BuyerChannels, @Campaigns, @FromPrice, @ToPrice

    SELECT b.BuyerChannelId, b.BuyerChannelName, CONVERT(decimal, b.BuyerPrice, 1) AS BuyerPrice, 
		   SUM(CASE WHEN b.[Status] = 1 THEN b.Quantity ELSE 0 END) AS SoldQuantity,
		   SUM(b.Quantity) AS Quantity, SUM(b.UQuantity) AS UQuantity

    FROM @buyerByPricesReport b
    GROUP BY b.BuyerChannelId, b.BuyerChannelName, CONVERT(decimal, b.BuyerPrice, 1)		
   
    END TRY
	  BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
				@SQLError_State    = ERROR_STATE(), 
				@SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	 END CATCH
   END