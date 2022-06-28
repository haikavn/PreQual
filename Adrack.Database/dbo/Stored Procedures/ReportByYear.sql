CREATE PROCEDURE [dbo].[ReportByYear]
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@parentid bigint,
	@campaignid bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	select t.* from (SELECT 'sold' as activity, sum (Quantity) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, sum(BuyerPrice) as buyerprice, sum(BuyerPrice) - sum(AffiliatePrice) as profit, sum(AffiliatePrice) as affiliateprice  
	FROM [dbo].[LeadMainReportDay] where created between @start and @end and [Status]=1 and (@campaignid = 0 or CampaignId = @campaignid) and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)))
	group by DATEPART(YEAR, created), DATEPART(MONTH, created)

	union

	SELECT 'received' as activity, cast(count (distinct LeadId) as int) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, 0 as buyerprice, 0 as profit, 0 as affiliateprice  
	FROM [dbo].[LeadMainReport] where [Status]!=0 and cast(Created as date) between @start and @end and (@campaignid = 0 or CampaignId = @campaignid) and  (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)))
	group by DATEPART(YEAR, Created), DATEPART(MONTH, Created)

	union

	SELECT 'posted' as activity, sum (Quantity) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, 0 as buyerprice, 0 as profit, 0 as affiliateprice  
	FROM [dbo].[LeadMainReportDay] where Created between @start and @end and (@campaignid = 0 or CampaignId = @campaignid) and  (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)))
	group by DATEPART(YEAR, Created), DATEPART(MONTH, Created)) t order by t.yr, t.mt
END