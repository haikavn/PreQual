
-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ReportByDate]
	@activity varchar(50),
	@start DateTime,
	@end DateTime,
	@delta int,
	@campaignId bigint,
	@parentid bigint

AS
BEGIN

	SET NOCOUNT ON;

	IF(@activity = 'sold')
	BEGIN
		SELECT 'sold' as activity, count (distinct[LeadId]) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, DATEPART(DAY, Created) as dy, DATEPART(HOUR, Created) as hr, ((DATEPART(MINUTE, Created) / @delta) * @delta) as mn, sum(BuyerPrice) as BuyerPrice, sum(BuyerPrice) - sum(AffiliatePrice) as profit, CAST(0 AS money) AS AffiliatePrice
		FROM [dbo].[LeadMainReport]
		WHERE Created between @start and @end and Status=1 and (@campaignId = 0 OR CampaignId = @campaignId) AND (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)))
		GROUP BY DATEPART(YEAR, Created), DATEPART(MONTH, Created), DATEPART(DAY, Created), DATEPART(HOUR, Created), ((DATEPART(MINUTE, Created) / @delta) * @delta)
		ORDER BY yr, mt, dy, hr, mn
	END

	IF(@activity = 'received')
	BEGIN
		SELECT 'received' as activity, count (distinct[LeadId]) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, DATEPART(DAY, Created) as dy, DATEPART(HOUR, Created) as hr, ((DATEPART(MINUTE, Created) / @delta) * @delta) as mn, CAST(0 AS money) as BuyerPrice, CAST(0 AS money) as profit, CAST(0 AS money) AS AffiliatePrice
		FROM [dbo].[LeadMainReport]
		WHERE Created between @start and @end and (@campaignId = 0 OR CampaignId = @campaignId) AND (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)))
		GROUP BY DATEPART(YEAR, Created), DATEPART(MONTH, Created), DATEPART(DAY, Created), DATEPART(HOUR, Created), ((DATEPART(MINUTE, Created) / @delta) * @delta)	
		ORDER BY yr, mt, dy, hr, mn
	END

	IF(@activity = 'posted')
	BEGIN
		SELECT 'posted' as activity, count ([LeadId]) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, DATEPART(DAY, Created) as dy, DATEPART(HOUR, Created) as hr, ((DATEPART(MINUTE, Created) / @delta) * @delta)	as mn, CAST(0 AS money) as BuyerPrice, CAST(0 AS money) as profit, CAST(0 AS money) AS AffiliatePrice
		FROM [dbo].[LeadMainReport]
		WHERE Created between @start and @end and (@campaignId = 0 OR CampaignId = @campaignId) AND (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)))
		GROUP BY DATEPART(YEAR, Created), DATEPART(MONTH, Created), DATEPART(DAY, Created), DATEPART(HOUR, Created), ((DATEPART(MINUTE, Created) / @delta) * @delta)
		ORDER BY yr, mt, dy, hr, mn
	END


END