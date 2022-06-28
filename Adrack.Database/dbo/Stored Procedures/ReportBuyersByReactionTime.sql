CREATE PROCEDURE [dbo].[ReportBuyersByReactionTime] 
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@BuyerId bigint,
	@campaigns varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @cids TABLE(
		Id bigint null
	);

	INSERT INTO @cids select Item from [dbo].SplitInts(@campaigns, ',');

	select cast(m.Created as Date) as Created, count(m.Id) as LeadViews, 
	min(datediff(second, m.SoldDate, m.ViewDate)) as MinElapsed, 
	avg(datediff(second, m.SoldDate, m.ViewDate)) as AvgElapsed,
	max(datediff(second, m.SoldDate, m.ViewDate)) as MaxElapsed
	from LeadMain m
	where
	m.BuyerChannelId = any(select Id from BuyerChannel where BuyerId = @BuyerId) and
	m.[Status] = 1 and m.SoldDate is not null and m.ViewDate is not null and 
	m.CampaignId in (select Id from @cids)
	group by cast(m.Created as Date);

	--select 0 as rownum, 0 as Id, '' as name, 0 as fid, 0 as totalleads, 0 as soldleads, cast(0 as money) as debet, cast(0 as money) as credit;
END