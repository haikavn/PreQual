CREATE PROCEDURE [dbo].[ReportAffiliatesByStates] 
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@affiliates varchar(MAX),
	@affiliateChannels varchar(MAX),
	@states varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare @condition varchar(MAX) = '';

	if LEN(@affiliates) > 0
	begin
		set @condition = ' and b.id in (' + @affiliates + ')';
	end;

	if LEN(@affiliateChannels) > 0
	begin
		set @condition = @condition + ' and r.affiliatechannelid in (' + @affiliateChannels + ')';
	end;

	if LEN(@states) > 0
	begin
		set @condition = @condition + ' and r.state in (' + @states + ')';
	end;
	
	declare @sql nvarchar(MAX);
	set @sql = 'SELECT   cast(ROW_NUMBER() OVER (Order by b.id) as int) AS rownum, b.id as AffiliateId, b.name as AffiliateName, r.state as state,
								 (SELECT        ISNULL(sum(quantity), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and campaigntype = 0 and affiliatechannelid=r.affiliatechannelid and affiliateid=b.id and state=r.state and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition + '
								   )  AS soldleads, 
								   (SELECT        ISNULL(sum(buyerprice), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and campaigntype = 0 and affiliatechannelid=r.affiliatechannelid and affiliateid=b.id and state=r.state and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition + '
								   )  AS debet,
								   (SELECT        ISNULL(sum(affiliateprice), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and campaigntype = 0 and affiliatechannelid=r.affiliatechannelid and affiliateid=b.id and state=r.state and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition + '
								   )  AS credit,	
								   (select count(u.Id) from RedirectUrl u inner join LeadMain m on m.Id = u.LeadId where m.affiliateid=b.id and m.AffiliateChannelId=r.AffiliateChannelId and cast(m.created as date) = cast(r.created as date) and m.created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + ' and u.Clicked = 1) as redirected,
								   ISNULL(sum(quantity), 0) AS totalleads
	FROM          dbo.LeadMainReportDayAffiliate r inner join dbo.affiliate b on b.id = r.affiliateid 
	where r.campaigntype = 0 and r.created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition + '
	GROUP BY b.id, b.name, r.state, r.created, r.affiliatechannelid order by r.state';

	exec(@sql);

	--select 0 as rownum, 0 as id, '' as name, 0 as fid, 0 as totalleads, 0 as soldleads, cast(0 as money) as debet, cast(0 as money) as credit;
END