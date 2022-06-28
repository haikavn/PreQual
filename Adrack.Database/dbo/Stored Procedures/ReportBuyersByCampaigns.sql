CREATE PROCEDURE [dbo].[ReportBuyersByCampaigns] 
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@buyers varchar(MAX),
	@buyerChannels varchar(MAX),
	@affiliateChannels varchar(MAX),
	@campaigns varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare @condition varchar(MAX) = '';
	declare @condition2 varchar(MAX) = '';

	if LEN(@buyers) > 0
	begin
		set @condition = ' and b.id in (' + @buyers + ')';
		set @condition2 = ' and buyerid in (' + @buyers + ')';
	end;

	if LEN(@buyerChannels) > 0
	begin
		set @condition = @condition + ' and r.BuyerChannelId in (' + @buyerChannels + ')';
		set @condition2 = @condition2 + ' and BuyerChannelId in (' + @buyerChannels + ')';
	end;

	if LEN(@affiliateChannels) > 0
	begin
		set @condition = @condition + ' and r.affiliatechannelid in (' + @affiliateChannels + ')';
		set @condition2 = @condition2 + ' and affiliatechannelid in (' + @affiliateChannels + ')';
	end;

	if LEN(@campaigns) > 0
	begin
		set @condition = @condition + ' and r.CampaignId in (' + @campaigns + ')';
		set @condition2 = @condition2 + ' and CampaignId in (' + @campaigns + ')';
	end;
	
	declare @sql nvarchar(MAX);
	set @sql = 'SELECT   cast(ROW_NUMBER() OVER (Order by b.id) as int) AS rownum, b.Id as BuyerId, b.Name as BuyerName, c.name as CampaignName,
								 (SELECT        ISNULL(sum(quantity), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and buyerid=b.id and campaignid=r.campaignid and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition2 + '
								   )  AS soldleads, 
								 (SELECT        ISNULL(sum(quantity), 0) AS Expr1
								   FROM            dbo.LeadMainReportDayRedirected where buyerid=b.id and campaignid=r.campaignid and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition2 + '
								   )  AS redirected, 
								   (SELECT        ISNULL(sum(BuyerPrice), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and buyerid=b.id and campaignid=r.campaignid and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition2 + '
								   )  AS debet,
								   (SELECT        ISNULL(sum(AffiliatePrice), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and buyerid=b.id and campaignid=r.campaignid and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition2 + '
								   )  AS credit,								   								   
								   ISNULL(sum(r.quantity), 0) AS totalleads
	FROM          dbo.LeadMainReportDay r inner join dbo.buyer b on b.id = r.buyerid inner join dbo.campaign c on c.id = r.campaignid 
	where r.campaigntype = 0 and r.Status != 7 and r.Status != 5 and r.created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition + '
	GROUP BY b.id, b.name, r.BuyerChannelId, c.name, r.campaignid order by b.id';

	exec(@sql);

	--select 0 as rownum, 0 as id, '' as name, 0 as fid, 0 as totalleads, 0 as soldleads, cast(0 as money) as debet, cast(0 as money) as credit;
END