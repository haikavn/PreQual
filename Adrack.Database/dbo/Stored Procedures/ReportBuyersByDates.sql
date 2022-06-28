CREATE PROCEDURE [dbo].[ReportBuyersByDates] 
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

	declare @TimeZone int = 0;

	select @TimeZone = cast([Value] as int) from Setting where [Key] = 'TimeZone';

	if @TimeZone is null 
	begin
		set @TimeZone = 0;
	end;

	declare @condition varchar(MAX) = '';
	declare @condition2 varchar(MAX) = '';
	declare @condition3 varchar(MAX) = '';

	if LEN(@buyers) > 0
	begin
		set @condition = ' and r.buyerid in (' + @buyers + ')';
		set @condition2 = ' and lr.buyerid in (' + @buyers + ')';
		set @condition3 = ' and buyerid in (' + @buyers + ')';
	end;

	if LEN(@buyerChannels) > 0
	begin
		set @condition = @condition + ' and r.buyerchannelid in (' + @buyerChannels + ')';
		set @condition2 = @condition2 + ' and lr.buyerchannelid in (' + @buyerChannels + ')';
		set @condition3 = @condition3 + ' and buyerchannelid in (' + @buyerChannels + ')';
	end;

	if LEN(@affiliateChannels) > 0
	begin
		set @condition = @condition + ' and r.affiliatechannelid in (' + @affiliateChannels + ')';
		set @condition2 = @condition2 + ' and lr.affiliatechannelid in (' + @affiliateChannels + ')';
		set @condition3 = @condition3 + ' and affiliatechannelid in (' + @affiliateChannels + ')';
	end;

	if LEN(@campaigns) > 0
	begin
		set @condition = @condition + ' and r.campaignid in (' + @campaigns + ')';
		set @condition2 = @condition2 + ' and lr.campaignid in (' + @campaigns + ')';
		set @condition3 = @condition3 + ' and campaignid in (' + @campaigns + ')';
	end;
	
	declare @sql nvarchar(MAX);
	set @sql = 'SELECT   r.created as date,
								 (SELECT        ISNULL(sum(quantity), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and campaigntype = 0 and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition3 + '
								   )  AS soldleads, 
								  (SELECT        ISNULL(sum(quantity), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 3 and campaigntype = 0 and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition3 + '
								   )  AS rejectedleads, 
								   (SELECT        ISNULL(sum(buyerprice), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and campaigntype = 0 and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition3 + '
								   )  AS debet,
								   (SELECT        ISNULL(sum(affiliateprice), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and campaigntype = 0 and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition3 + '
								   )  AS credit,								   								   
								   (SELECT        ISNULL(sum(buyerprice)/sum(quantity), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and campaigntype = 0 and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition3 + '
								   )  AS averageprice,
								   ISNULL(sum(r.quantity), 0) AS totalleads,
								   (SELECT        ISNULL(sum(quantity), 0) AS Expr1
								   FROM            dbo.LeadMainReportDayRedirected where campaigntype = 0 and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition3 + '
								   )  AS redirected,	
								   (select ISNULL(count(lr.LeadId), 0) from LeadMainReport lr inner join LeadNote n on n.LeadId=lr.LeadId where cast(dateadd(hour, ' + cast(@TimeZone as varchar(10)) + ', lr.created) as date) = r.created and  lr.created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition2 + ') as loanedleads
	FROM          dbo.LeadMainReportDay r
	where r.campaigntype = 0 and r.Status != 7 and r.Status != 5 and r.created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition + '
	GROUP BY r.created order by r.created';

	exec(@sql);
END