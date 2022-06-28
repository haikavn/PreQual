CREATE PROCEDURE [dbo].[ReportBuyersByCampaignsFinal]
    @Start         DATETIME,
	@End           DATETIME,
	@Buyers        VARCHAR(MAX),
	@BuyerChannels VARCHAR(MAX),
	@AffiliateChannels VARCHAR(MAX),
	@Campaigns     VARCHAR(MAX)
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

   DECLARE @buyerByCampaigns TABLE ( RowNum bigint not null, BuyerId bigint not null, BuyerName varchar(50), CampaignName varchar(50),
						              SoldLeads int not null, Redirected int not null, Debet decimal(18,2), Credit decimal(18,2), 
									  TotalLeads int not null )

   INSERT @buyerByCampaigns EXEC dbo.ReportBuyersByCampaigns @Start, @End,  @Buyers, @BuyerChannels, @AffiliateChannels, @Campaigns

   SELECT b.BuyerId, b.BuyerName, b.CampaignName, b.TotalLeads, b.SoldLeads, b.Debet, b.Credit, b.Redirected,
	      CONVERT(decimal(18,2), CASE WHEN b.SoldLeads > 0 THEN 
				  ROUND( Convert( decimal, b.Redirected) / Convert( decimal, b.SoldLeads) * 100, 2) ELSE 0 END) AS RedirectedRate,
          CONVERT(decimal(18,2), CASE WHEN b.TotalLeads > 0 THEN 
				  ROUND( Convert( decimal, b.SoldLeads) / Convert( decimal, b.TotalLeads) * 100, 2) ELSE 0 END) AS AcceptRate
   FROM @buyerByCampaigns b
   UNION ALL
   SELECT  0, NULL, NULL, SUM(tb.TotalLeads), SUM(tb.SoldLeads), SUM(tb.Debet), SUM(tb.Credit), SUM(tb.Redirected),
	       CONVERT(decimal(18,2), CASE WHEN SUM(tb.SoldLeads) > 0 THEN 
				   ROUND( Convert( decimal, SUM(tb.Redirected)) / Convert( decimal, SUM(tb.SoldLeads)) * 100, 2) ELSE 0 END),
           CONVERT(decimal(18,2), CASE WHEN SUM(tb.TotalLeads) > 0 THEN 
				  ROUND( Convert( decimal, SUM(tb.SoldLeads)) / Convert( decimal, SUM(tb.TotalLeads)) * 100, 2) ELSE 0 END)
   FROM @buyerByCampaigns tb
END TRY
		BEGIN CATCH
			SELECT @SQLError_Severity = ERROR_SEVERITY(), 
				   @SQLError_State    = ERROR_STATE(), 
				   @SQLError_Message  = ERROR_MESSAGE()

			RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
		END CATCH
END