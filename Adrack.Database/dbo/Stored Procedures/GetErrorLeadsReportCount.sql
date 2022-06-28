
-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetErrorLeadsReportCount]
	-- Add the parameters for the stored procedure here
	@errorType smallint = 0,
	@validator smallint = 0,
	@dateFrom datetime,
	@dateTo datetime,
	@AffiliateId bigint = 0,
	@AffiliateChannelId bigint = 0,
	@BuyerId bigint = 0,
	@BuyerChannelId bigint = 0,
	@campaignId bigint = 0,
	@state varchar(50) = '',
	@minPrice money = 0,
	@reportType smallint = 1,
	@leadId bigint = 0,
	@status smallint = 0
AS

if @reportType = 1
begin
	select count(lm.Id)
	from dbo.LeadMainResponse lm inner join LeadContent lc on lc.LeadId = lm.LeadId inner join Affiliate a on a.Id = lm.AffiliateId inner join AffiliateChannel ac on ac.Id = lm.AffiliateChannelId inner join Buyer b on b.Id = lm.BuyerId inner join BuyerChannel bc on bc.Id = lm.BuyerChannelId
	where lm.Created between @dateFrom and @dateTo and (@errorType = 0 or (@errorType > 0 and lm.ErrorType = @errorType) or (@errorType = -1 and (lm.ErrorType = 17 or lm.ErrorType = 18 or lm.Response = 'Channel timeout'))) and (@validator = 0 or (@validator = lm.Validator))
	and (@AffiliateId = 0 or (@AffiliateId > 0 and lm.AffiliateId = @AffiliateId)) and (@AffiliateChannelId = 0 or (@AffiliateChannelId > 0 and lm.AffiliateChannelId = @AffiliateChannelId))
	and (@BuyerId = 0 or (@BuyerId > 0 and lm.BuyerId = @BuyerId)) and (@BuyerChannelId = 0 or (@BuyerChannelId > 0 and lm.BuyerChannelId = @BuyerChannelId))
	and (len(@state) = 0 or lm.[State] = @state)
	and (@status = 0 or (@status > 0 and lm.Status = @status))
	and (@campaignId = 0 or (@campaignId > 0 and lm.CampaignId = @campaignId))
end
else
begin
	select count(lm.Id)
	from dbo.AffiliateResponse lm inner join LeadContent lc on lc.LeadId = lm.LeadId inner join Affiliate a on a.Id = lm.AffiliateId inner join AffiliateChannel ac on ac.Id = lm.AffiliateChannelId
	where lm.Created between @dateFrom and @dateTo and (@errorType = 0 or ((@errorType = 0 and lm.ErrorType > 0) and lm.ErrorType = @errorType) or (@errorType = -1 and (lm.ErrorType = 17 or lm.ErrorType = 18 or lm.Response = 'Channel timeout'))) and (@validator = 0 or (@validator = lm.Validator))
	and (@AffiliateId = 0 or (@AffiliateId > 0 and lm.AffiliateId = @AffiliateId))and (@AffiliateChannelId = 0 or (@AffiliateChannelId > 0 and lm.AffiliateChannelId = @AffiliateChannelId))
	and (len(@state) = 0 or lm.[State] = @state)
	and (@status = 0 or (@status > 0 and lm.Status = @status))
end;