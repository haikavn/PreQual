CREATE PROCEDURE [dbo].[ReportBuyersWinRateReport] 
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@buyerChannels varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare @condition varchar(MAX) = '';
	declare @condition2 varchar(MAX) = '';
	declare @condition3 varchar(MAX) = '';


	if LEN(@buyerChannels) > 0
	begin
		set @condition = @condition + ' and r.buyerchannelid in (' + @buyerChannels +')';
		set @condition2 = @condition2 + ' and buyerchannelid in (' + @buyerChannels + ')';
		set @condition3 = @condition3 + ' and buyerchannelid not in (' + @buyerChannels + ')';
	end;
	
	declare @sql nvarchar(MAX);
	set @sql = 'SELECT    b.id as BuyerChannelId, b.name as BuyerChannelName,
								 (SELECT        ISNULL(sum(quantity), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and campaigntype = 0 and buyerchannelid=r.buyerchannelid and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition2 + '
								   )  AS soldleads, 
								   (SELECT        ISNULL(sum(buyerprice), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and campaigntype = 0 and buyerchannelid=r.buyerchannelid and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition2 + '
								   )  AS BuyerPrice,
								   (SELECT        ISNULL(sum(affiliateprice), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 1 and campaigntype = 0 and buyerchannelid=r.buyerchannelid and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition2 + '
								   )  AS AffiliatePrice,								   								   
								   (SELECT        ISNULL(sum(quantity), 0) AS Expr1
								   FROM            dbo.LeadMainReportDay where status = 3 and campaigntype = 0 and buyerchannelid=r.buyerchannelid and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition2 + '
								   )  AS rejectedleads,
								   (SELECT        ISNULL(count(distinct leadid), 0) AS Expr1
								   FROM            dbo.LeadMainReponse where status = 3 and ErrorType = 14 and campaigntype = 0 and buyerchannelid!=r.buyerchannelid and created = r.created and created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition3 + '
								   )  AS MinPriceErrorLeads, 
								   ISNULL(sum(r.quantity), 0) AS totalleads
	FROM          dbo.LeadMainReportDay r inner join dbo.buyerchannel b on b.id = r.buyerchannelid 
	where r.campaigntype = 0 and r.Status != 7 and r.Status != 5 and r.created between ''' + cast(@start as varchar(20)) + ''' and ''' + cast(@end as varchar(20)) +  '''' + @condition + '
	GROUP BY b.id, b.name, r.buyerid, r.campaignid, r.BuyerChannelId, r.created order by b.name';

	exec(@sql);

	--select 0 as rownum, 0 as id, '' as name, 0 as fid, 0 as totalleads, 0 as soldleads, cast(0 as money) as debet, cast(0 as money) as credit;
END