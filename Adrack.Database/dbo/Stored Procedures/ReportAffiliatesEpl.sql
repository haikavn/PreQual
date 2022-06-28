-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE ReportAffiliatesEpl
	@start DateTime,
	@end DateTime,
	@affiliates varchar(MAX),
	@affiliateChannels varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ids TABLE(
		Id bigint null
	);

	INSERT INTO @ids select Item from [dbo].SplitInts(@affiliates, ',');

	DECLARE @ids2 TABLE(
		Id bigint null
	);

	INSERT INTO @ids2 select Item from [dbo].SplitInts(@affiliateChannels, ',');

	select r.AffiliateId, r.AffiliateChannelId, a.Name, ac.Name, sum(r.Quantity) as Total, 
	(
		select sum(Quantity) from LeadMainReportDay 
		where Created between @start and @end and 
		AffiliateId in (select Id from @ids) and 
		AffiliateChannelId in (select Id from @ids2) and Status = 1
	) as Sold,
	(
		select count(u.Id) from RedirectUrl u inner join LeadMain m on m.Id = u.LeadId where m.Created between @start and @end and 
		m.AffiliateId in (select Id from @ids) and 
		m.AffiliateChannelId in (select Id from @ids2) and u.Clicked = 1
	) as Redirects,
	(
		select sum(BuyerPrice - AffiliatePrice) from LeadMainReportDay 
		where Created between @start and @end and 
		AffiliateId in (select Id from @ids) and 
		AffiliateChannelId in (select Id from @ids2) and Status = 1
	) as Profit
	from LeadMainReportDayAffiliate r inner join Affiliate a on a.Id = r.AffiliateId inner join AffiliateChannel ac on ac.Id = r.AffiliateChannelId
	group by r.AffiliateId, r.AffiliateChannelId, a.Name, ac.Name;

END