-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ReportBuyersByLeadNotes]
	-- Add the parameters for the stored procedure here
	@start Datetime,
	@end DateTime,
	@buyers varchar(max),
	@buyerChannels varchar(max),
	@affiliateChannels varchar(max),
	@campaigns varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
	DECLARE @ids TABLE(
		Id bigint null
	);

	INSERT INTO @ids select Item from [dbo].SplitInts(@buyers, ',');

	DECLARE @ids2 TABLE(
		Id bigint null
	);

	INSERT INTO @ids2 select Item from [dbo].SplitInts(@buyerChannels, ',');


	DECLARE @ids3 TABLE(
		Id bigint null
	);

	INSERT INTO @ids3 select Item from [dbo].SplitInts(@affiliateChannels, ',');

	DECLARE @cids TABLE(
		Id bigint null
	);

	INSERT INTO @cids select Item from [dbo].SplitInts(@campaigns, ',');

	select cast(n.Created as Date) as Created, t.Title, c.[Name] as ChannelName, b.[Name] as BuyerName, count(n.LeadId) as Quantity from LeadNote n inner join NoteTitle t on t.Id = n.NoteTitleId
	inner join LeadMainReport r on n.LeadId=r.LeadId and r.BuyerId in (select Id from @ids) and r.BuyerChannelId in (select Id from @ids2) and r.AffiliateChannelId in (select Id from @ids3) and r.CampaignId in (select Id from @cids)
	inner join BuyerChannel c on c.Id = r.BuyerChannelId inner join Buyer b on b.Id = c.BuyerId
    where n.Created between @start and @end
	group by cast(n.Created as Date), t.Title, c.[Name], b.[Name]  order by cast(n.Created as Date), t.Title


    -- Insert statements for procedure here
	--SELECT * from [dbo].SplitInts(@buyers, ',');
END