
-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetErrorLeadsReport]
	-- Add the parameters for the stored procedure here
	@errorType smallint = 0,
	@validator smallint = 0,
	@dateFrom datetime,
	@dateTo datetime,
	@AffiliateId bigint = 0,
	@AffiliateChannelId bigint = 0,
	@BuyerId bigint = 0,
	@BuyerChannelId bigint = 0,
	@CampaignId bigint = 0,
	@state varchar(50) = '',
	@minPrice money = 0,
	@reportType smallint = 1,
	@leadId bigint = 0,
	@status smallint = 0,
	@start int,
	@count int = 1
AS

if @reportType = 1
begin
	select lm.LeadId as LeadId, 
	lm.Created, 
	lm.AffiliateId, 
	lm.AffiliateChannelId, 
	lm.BuyerId, 
	lm.BuyerChannelId, 
	lm.CampaignId,
	lm.ErrorType, 
	lm.Validator, 
	a.Name as AffiliateName, 
	ac.Name as AffiliateChannelName, 
	b.Name as BuyerName, 
	bc.Name as BuyerChannelName,
	c.Name as CampaignName,
	lm.Response,
	lm.ResponseError as [Message],
	lm.State,
	lm.Status,
	lc.Minprice
	from dbo.LeadMainResponse lm inner join LeadContent lc on lc.LeadId = lm.LeadId inner join Affiliate a on a.Id = lm.AffiliateId inner join AffiliateChannel ac on ac.Id = lm.AffiliateChannelId inner join Buyer b on b.Id = lm.BuyerId inner join BuyerChannel bc on bc.Id = lm.BuyerChannelId inner join Campaign c on c.Id = lm.CampaignId
	where lm.status != 7 and lm.Created between @dateFrom and @dateTo and (@errorType = 0 or (@errorType > 0 and lm.ErrorType = @errorType) or (@errorType = -1 and (lm.ErrorType = 17 or lm.ErrorType = 18 or lm.Response = 'Channel timeout'))) and (@validator = 0 or (@validator = lm.Validator))
	and (@AffiliateId = 0 or (@AffiliateId > 0 and lm.AffiliateId = @AffiliateId)) and (@AffiliateChannelId = 0 or (@AffiliateChannelId > 0 and lm.AffiliateChannelId = @AffiliateChannelId))
	and (@BuyerId = 0 or (@BuyerId > 0 and lm.BuyerId = @BuyerId)) and (@BuyerChannelId = 0 or (@BuyerChannelId > 0 and lm.BuyerChannelId = @BuyerChannelId)) and (@minPrice = 0 or lc.Minprice = @minPrice)
	and (len(@state) = 0 or lm.[State] = @state)
	and (@status = 0 or (@status > 0 and lm.Status = @status))
	and (@campaignId = 0 or (@campaignId > 0 and lm.CampaignId = @CampaignId))
	ORDER BY lm.Created Desc
	OFFSET @start ROWS FETCH NEXT @count ROWS ONLY;
end
else
begin
	select ISNULL(lm.LeadId, 0) as LeadId, 
	lm.Created, 
	lm.AffiliateId, 
	lm.AffiliateChannelId, 
	0 as CampaignId,
	lm.ErrorType, 
	lm.Validator, 
	a.Name as AffiliateName, 
	ac.Name as AffiliateChannelName, 
	'' as BuyerName, 
	'' as BuyerChannelName, 
	'' as CampaignName,
	lm.Response, 
	lm.[Message], 
	lm.State,
	lm.Status as [Status],
	lc.Minprice
	from dbo.AffiliateResponse lm inner join LeadContent lc on lc.LeadId = lm.LeadId inner join Affiliate a on a.Id = lm.AffiliateId inner join AffiliateChannel ac on ac.Id = lm.AffiliateChannelId
	where lm.status != 7 and lm.Created between @dateFrom and @dateTo and ((@errorType = 0 and lm.ErrorType > 0) or (@errorType > 0 and lm.ErrorType = @errorType) or (@errorType = -1 and (lm.ErrorType = 17 or lm.ErrorType = 18 or lm.Response = 'Channel timeout'))) and (@validator = 0 or (@validator = lm.Validator))
	and (@AffiliateId = 0 or (@AffiliateId > 0 and lm.AffiliateId = @AffiliateId))and (@AffiliateChannelId = 0 or (@AffiliateChannelId > 0 and lm.AffiliateChannelId = @AffiliateChannelId))
	and (len(@state) = 0 or lm.[State] = @state) and (@minPrice = 0 or lc.Minprice = @minPrice)
	and (@status = 0 or (@status > 0 and lm.Status = @status))
	ORDER BY lm.Created Desc
	OFFSET @start ROWS FETCH NEXT @count ROWS ONLY;
end;