CREATE PROCEDURE [dbo].[ReportBuyersWinRateReportFinal]
( 
    @start DateTime,
	@end DateTime,
	@buyerChannels varchar(MAX)
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

    DECLARE @buyersWinRateReport TABLE (BuyerChannelId bigint not null, BuyerChannelName varchar(50),
	                                    BuyerPrice decimal(18,2), TotalLeads int not null, SoldLeads int not null,
										RejectedLeads int not null, MinPriceErrorLeads bigint not null)

   INSERT @buyersWinRateReport EXEC dbo.ReportBuyersWinRateReport @start, @end, @buyerChannels

   SELECT b.BuyerPrice, SUM(b.TotalLeads) AS TotalLeads, SUM(b.SoldLeads) AS SoldLeads, SUM(b.RejectedLeads) AS RejectedLeads,
	      SUM(b.MinPriceErrorLeads) AS MinPriceErrorLeads, SUM(b.BuyerPrice) AS TotalBuyerPrice
   FROM @buyersWinRateReport b
   GROUP BY b.BuyerPrice		
   UNION ALL
   SELECT 0, SUM(b.TotalLeads), SUM(b.SoldLeads), SUM(b.RejectedLeads),
	      SUM(b.MinPriceErrorLeads), SUM(b.BuyerPrice)
   FROM @buyersWinRateReport b
   END TRY
	 BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
				@SQLError_State    = ERROR_STATE(), 
				@SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	 END CATCH
   END
