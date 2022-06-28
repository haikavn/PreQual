CREATE PROCEDURE [dbo].[ReportByMinutes]
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@parentid bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @k int = 10;

	
	select t.* from (SELECT 'sold' as activity, count (distinct[LeadId]) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, DATEPART(DAY, Created) as dy, DATEPART(HOUR, Created) as hr, ((DATEPART(MINUTE, Created) / @k) * @k) as mn, sum(BuyerPrice) as buyerprice, sum(BuyerPrice) - sum(AffiliatePrice) as profit, sum(AffiliatePrice) as affiliateprice   
	FROM [dbo].[LeadMainReport] where Created between @start and @end and Status=1 and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid))) group by DATEPART(YEAR, Created), DATEPART(MONTH, Created), DATEPART(DAY, Created), DATEPART(HOUR, Created), ((DATEPART(MINUTE, Created) / @k) * @k)
	union
	SELECT 'received' as activity, count (distinct[LeadId]) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, DATEPART(DAY, Created) as dy, DATEPART(HOUR, Created) as hr, ((DATEPART(MINUTE, Created) / @k) * @k) as mn, 0 as buyerprice, 0 as profit, 0 as affiliateprice  
	FROM [dbo].[LeadMainReport] where Created between @start and @end and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid))) group by DATEPART(YEAR, Created), DATEPART(MONTH, Created), DATEPART(DAY, Created), DATEPART(HOUR, Created), ((DATEPART(MINUTE, Created) / @k) * @k)		
	union
	SELECT 'posted' as activity, count ([LeadId]) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, DATEPART(DAY, Created) as dy, DATEPART(HOUR, Created) as hr, ((DATEPART(MINUTE, Created) / @k) * @k)	as mn, 0 as buyerprice, 0 as profit, 0 as affiliateprice 
	FROM [dbo].[LeadMainReport] where Created between @start and @end and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid))) group by DATEPART(YEAR, Created), DATEPART(MONTH, Created), DATEPART(DAY, Created), DATEPART(HOUR, Created), ((DATEPART(MINUTE, Created) / @k) * @k)) t order by t.yr, t.mt, t.dy, t.hr, t.mn, t.activity


	


END