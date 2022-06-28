CREATE PROCEDURE [dbo].[ReportBuyersByDatesFinal]
	(
    @start DateTime,
	@end DateTime,
	@buyers varchar(MAX),
	@buyerChannels varchar(MAX),
	@affiliateChannels varchar(MAX),
	@campaigns varchar(MAX)
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

    DECLARE @buyerByDatesReport TABLE ([Date] date, SoldLeads int not null,
									   Debet decimal(18,2), Credit decimal(18,2), 
									   AveragePrice decimal(18,4) not null, TotalLeads int not null,
									   Redirected int not null, LoanedLeads int not null)

   INSERT @buyerByDatesReport EXEC dbo.ReportBuyersByDates @Start, @End,  @Buyers, @BuyerChannels, @AffiliateChannels, @Campaigns

    SELECT b.[Date], SUM(b.TotalLeads) AS TotalLeads, SUM(b.SoldLeads) AS SoldLeads, 
           SUM(b.Debet) AS Debet, SUM(b.Credit) AS Credit, SUM(b.Redirected) AS Redirected, SUM(b.LoanedLeads) AS LoanedLeads,
	       CONVERT(decimal(18,2), CASE WHEN SUM(b.SoldLeads) > 0 THEN 
				   ROUND( Convert( decimal, SUM(b.Redirected)) / Convert( decimal, SUM(b.SoldLeads)) * 100, 2) ELSE 0 END) AS RedirectedRate,
		   SUM(b.Debet - b.Credit) AS Profit, SUM(b.AveragePrice) AS AveragePrice,
           CONVERT(decimal(18,2), CASE WHEN SUM(b.TotalLeads) > 0 THEN 
				   ROUND( Convert( decimal, SUM(b.SoldLeads)) / Convert( decimal, SUM(b.TotalLeads)) * 100, 2) ELSE 0 END) AS AcceptRate
   FROM @buyerByDatesReport b
   GROUP BY b.[Date]		
   UNION ALL
    SELECT NULL, SUM(tb.TotalLeads), SUM(tb.SoldLeads), 
           SUM(tb.Debet), SUM(tb.Credit), SUM(tb.Redirected),  SUM(tb.LoanedLeads),
	       CONVERT(decimal(18,2), CASE WHEN SUM(tb.SoldLeads) > 0 THEN 
				   ROUND( Convert( decimal, SUM(tb.Redirected)) / Convert( decimal, SUM(tb.SoldLeads)) * 100, 2) ELSE 0 END),
		   SUM(tb.Debet - tb.Credit), SUM(tb.AveragePrice),
           CONVERT(decimal(18,2), CASE WHEN SUM(tb.TotalLeads) > 0 THEN 
				   ROUND( Convert( decimal, SUM(tb.SoldLeads)) / Convert( decimal, SUM(tb.TotalLeads)) * 100, 2) ELSE 0 END)
   FROM @buyerByDatesReport tb
   END TRY
	 BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
				@SQLError_State    = ERROR_STATE(), 
				@SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	 END CATCH
   END
