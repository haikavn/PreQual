CREATE PROCEDURE [dbo].[ReportByDays]
	-- Add the parameters for the stored procedure here
/*
	@now DateTime,
*/
	@start DateTime,
	@end DateTime,
	@campaignid bigint,
	@parentid bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	declare @TimeZone int = 0;

	select @TimeZone = cast([Value] as int) from Setting where [Key] = 'TimeZone';

	if @TimeZone is null 
	begin
		set @TimeZone = 0;
	end;

	select r.Created, ISNULL(sum(r.Quantity), 0) as total, (select ISNULL(sum(Quantity), 0)
	from LeadMainReportDay
	where Status = 1 and (@campaignid = 0 OR CampaignId=@campaignid) and Created = r.Created and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)))) as sold,
	(select ISNULL(sum(AffiliatePrice), 0)
	from LeadMainReportDay where Status = 1 and (@campaignid = 0 OR CampaignId=@campaignid) and Created = r.Created and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)))) as AffiliatePrice,
	(select ISNULL(sum(BuyerPrice), 0)
	from LeadMainReportDay where Status = 1 and (@campaignid = 0 OR CampaignId=@campaignid) and Created = r.Created and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)))) as BuyerPrice, 
	(select ISNULL(sum(BuyerPrice - AffiliatePrice), 0) from LeadMainReportDay where Status = 1 and Created = r.Created and (@campaignid = 0 OR CampaignId=@campaignid) and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)))) as profit,
	(select ISNULL(sum(Quantity), 0) from LeadMainReportDayReceived where Created = r.Created and (@campaignid = 0 OR CampaignId=@campaignid) and (@parentid = 0 or (@parentid < 0 and AffiliateId = abs(@parentid)))) as received
	from LeadMainReportDay r
	where r.Created between @start and @end and (@campaignid = 0 OR r.CampaignId=@campaignid) and (@parentid = 0 or (@parentid > 0 and r.BuyerId = @parentid) or (@parentid < 0 and r.AffiliateId = abs(@parentid)))
	group by r.Created;

END