-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[ReportBuyersComparisonBuyerChannels]
(
    -- Add the parameters for the stored procedure here
    @buyerchannelid bigint,
    @campaignid bigint,
	@date1 date,
	@date2 date,
	@date3 date
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
	select c.Id as CampaignId, 
	   c.Name as CampaignName,
	   a.Id as AffiliateId,
	   a.Name as AffiliateName,
	   ac.Id as AffiliateChannelId,
	   ac.Name as AffiliatechannelName,
	   bc.Id as BuyerId,
	   bc.Name as BuyerName,
	   rd.Created,
	   (select ISNULL(sum(Quantity), 0) from LeadMainReportDay where CampaignId = rd.CampaignId and AffiliateId = rd.AffiliateId and AffiliateChannelId = rd.AffiliateChannelId and Created = rd.Created and Status = 1) as Sold,
	   (select ISNULL(sum(Quantity), 0) from LeadMainReportDay where CampaignId = rd.CampaignId and AffiliateId = rd.AffiliateId and AffiliateChannelId = rd.AffiliateChannelId and Created = rd.Created and Status = 3) as Rejected,
	   (select ISNULL(sum(Quantity), 0) from LeadMainReportDayRedirected where CampaignId = rd.CampaignId and AffiliateId = rd.AffiliateId and AffiliateChannelId = rd.AffiliateChannelId and Created = rd.Created) as Redirected,
   	   (select ISNULL(sum(AffiliatePrice), 0) from LeadMainReportDay where CampaignId = rd.CampaignId and AffiliateId = rd.AffiliateId and AffiliateChannelId = rd.AffiliateChannelId and Created = rd.Created and Status = 1) as Revenue,
	   sum(rd.Quantity) as Posted
	   
	   from LeadMainReportDay rd 
			  inner join Campaign c on c.Id = rd.CampaignId
			  inner join Affiliate a on a.Id = rd.AffiliateId
			  inner join AffiliateChannel ac on ac.Id = rd.AffiliateChannelId
			  left join BuyerChannel bc on bc.Id = rd.BuyerChannelId
			  where (@buyerchannelid = 0 or rd.BuyerChannelId = @buyerchannelid) and (@campaignid = 0 or rd.CampaignId = @campaignid) and (rd.Created = @date1 or rd.Created = @date2 or rd.Created = @date3)
			  group by 
			  rd.CampaignId, rd.AffiliateId, rd.AffiliateChannelId, bc.Id, bc.Name, c.Id, c.Name, a.Id, a.Name, ac.Id, ac.Name, rd.Created
			  order by rd.Created
END