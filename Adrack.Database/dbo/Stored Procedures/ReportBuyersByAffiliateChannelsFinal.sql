CREATE PROCEDURE [dbo].[ReportBuyersByAffiliateChannelsFinal]
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

    DECLARE @buyerByAffiliateChanelReport TABLE ( RowNum bigint not null, BuyerId bigint not null, BuyerName varchar(50),
	                                              AffiliateChannelId bigint not null, AffiliateChannelName varchar(50),
						                          SoldLeads int not null, Debet decimal(18,2), Credit decimal(18,2), 
									              AveragePrice decimal(18,4), Redirected int not null, TotalLeads int not null )

   INSERT @buyerByAffiliateChanelReport EXEC dbo.ReportBuyersByAffiliateChannels @Start, @End, @Buyers, @BuyerChannels, @AffiliateChannels, @Campaigns

   SELECT b.AffiliateChannelId, b.AffiliateChannelName, SUM(b.TotalLeads) AS TotalLeads, SUM(b.SoldLeads) AS SoldLeads, 
          SUM(b.Debet) AS Debet, SUM(b.Credit) AS Credit, SUM(b.Redirected) AS Redirected,
	      CONVERT(decimal(18,2), CASE WHEN SUM(b.SoldLeads) > 0 THEN 
				  ROUND( Convert( decimal, SUM(b.Redirected)) / Convert( decimal, SUM(b.SoldLeads)) * 100, 2) ELSE 0 END) AS RedirectedRate,
		  SUM(b.Debet -b.Credit) AS Profit, SUM(b.AveragePrice) AS AveragePrice,
          CONVERT(decimal(18,2), CASE WHEN SUM(b.TotalLeads) > 0 THEN 
				  ROUND( Convert( decimal, SUM(b.SoldLeads)) / Convert( decimal, SUM(b.TotalLeads)) * 100, 2) ELSE 0 END) AS AcceptRate
   FROM @buyerByAffiliateChanelReport b
   GROUP BY b.AffiliateChannelId, b.AffiliateChannelName
			
   UNION ALL
   SELECT 0, NULL, SUM(tb.TotalLeads), SUM(tb.SoldLeads), SUM(tb.Debet), SUM(tb.Credit), SUM(tb.Redirected),
	      CONVERT(decimal(18,2), CASE WHEN SUM(tb.SoldLeads) > 0 THEN 
				  ROUND( Convert( decimal, SUM(tb.Redirected)) / Convert( decimal, SUM(tb.SoldLeads)) * 100, 2) ELSE 0 END),
		  SUM(tb.Debet - tb.Credit), SUM(tb.AveragePrice),
          CONVERT(decimal(18,2), CASE WHEN SUM(tb.TotalLeads) > 0 THEN 
				  ROUND( Convert( decimal, SUM(tb.SoldLeads)) / Convert( decimal, SUM(tb.TotalLeads)) * 100, 2) ELSE 0 END) 
   FROM @buyerByAffiliateChanelReport tb
END TRY
	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
				@SQLError_State    = ERROR_STATE(), 
				@SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END
