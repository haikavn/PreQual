-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[ReportTotals_OLD]
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@parentid bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @now DateTime = getdate();--convert(datetimeoffset, getdate()) AT TIME ZONE (select top 1 Value from setting where [Key]='TimeZone');

	declare @start2 DateTime = @start;
	declare @end2 DateTime = convert(nvarchar(10), cast(@end as Date), 10) + ' ' + cast(cast(@now as Time) as varchar(10));

    -- Insert statements for procedure here

	declare @starttime Time = '00:00:00';
	declare @endtime Time = '23:59:59';

	--declare @start as DateTime = convert(nvarchar(10), cast(@now as Date), 10) + ' 00:00:00';
	--declare @end as DateTime = convert(nvarchar(10), cast(@now as Date), 10) + ' 23:59:59';

	declare @receivedleads_today int = 0;
	declare @totalleads_today int = 0;
	declare @soldleads_today int = 0;
	declare @debit_today money = 0;
	declare @profit_today money = 0;

	select @receivedleads_today = ISNULL(count(distinct LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid))) ;
	select @totalleads_today = ISNULL(count(LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid))) ;
	select @soldleads_today = ISNULL(count(LeadId), 0), @debit_today = ISNULL(sum(BuyerPrice), 0), @profit_today = ISNULL(sum(BuyerPrice - AffiliatePrice), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and Status = 1 and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	--select @credit_today = ISNULL(sum(aprice), 0) from r_main where Created between @start and @now  and Status = 1;
	
	----- yesterday -----

	set @start = DATEADD(day, -1, @start2);
	set @end = DATEADD(day, -1, @end2);

	print @start;
	print @end;

	declare @receivedleads_yesterday int = 0;
	declare @totalleads_yesterday int = 0;
	declare @soldleads_yesterday int = 0;
	declare @debit_yesterday money = 0;
	declare @profit_yesterday money = 0;

	select @receivedleads_yesterday = ISNULL(count(distinct LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid))) ;
	select @totalleads_yesterday = ISNULL(count(LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	select @soldleads_yesterday = ISNULL(count(LeadId), 0), @debit_yesterday = ISNULL(sum(BuyerPrice), 0), @profit_yesterday = ISNULL(sum(BuyerPrice - AffiliatePrice), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and Status = 1 and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	--select @credit_yesterday = ISNULL(sum(aprice), 0) from r_main where Created between @start and @end  and Status = 1;

	----- 7 days -----

	set @start = DATEADD(day, -7, @start2);
	set @end = DATEADD(day, -7, @end2);

	declare @receivedleads_7days int = 0;
	declare @totalleads_7days int = 0;
	declare @soldleads_7days int = 0;
	declare @debit_7days money = 0;
	declare @profit_7days money = 0;

	select @receivedleads_7days = ISNULL(count(distinct LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	select @totalleads_7days = ISNULL(count(LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	select @soldleads_7days = ISNULL(count(LeadId), 0), @debit_7days = ISNULL(sum(BuyerPrice), 0), @profit_7days = ISNULL(sum(BuyerPrice - AffiliatePrice), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and Status = 1 and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	--select @credit_7days = ISNULL(sum(aprice), 0) from r_main where Created between @start and @end  and Status = 1;	

	select t.* from (
		select 1 as num, 'Today' as name, @receivedleads_today as received, @totalleads_today as total, 
		(case when @receivedleads_yesterday > 0 then cast ( ((cast(@receivedleads_today as float) - cast(@receivedleads_yesterday as float)) / cast(@receivedleads_yesterday as float) * 100) as decimal(10, 2)) else cast(0.0 as decimal(10, 2)) end) as receivedp, 

		(case when @totalleads_yesterday > 0 then cast( ((cast(@totalleads_today as float) - cast(@totalleads_yesterday as float)) / cast(@totalleads_yesterday as float) * 100) as decimal(10, 2)) else cast(0.0 as decimal(10, 2)) end) as totalp,
		
		@soldleads_today as sold, 
		(case when @soldleads_yesterday > 0 then cast( ((cast(@soldleads_today as float) - cast(@soldleads_yesterday as float)) / cast(@soldleads_yesterday as float) * 100) as decimal(10, 2)) else cast(0.0 as decimal(10, 2)) end) as soldp,
		@debit_today as debit,
		(case when @debit_yesterday > 0 then cast( ((cast(@debit_today as float) - cast(@debit_yesterday as float)) / cast(@debit_yesterday as float) * 100) as decimal(10, 2)) else cast(0.0 as decimal(10, 2)) end) as debitp,

		@profit_today as profit,
		(case when @profit_yesterday > 0 then cast( ((cast(@profit_today as float) - cast(@profit_yesterday as float)) / cast(@profit_yesterday as float) * 100) as decimal(10, 2)) else cast(0.0 as decimal(10, 2)) end) as profitp

		union
		select 2 as num, 'Today' as name, @receivedleads_today as received, @totalleads_today as total, 
		(case when @receivedleads_7days > 0 then cast ( ((cast(@receivedleads_today as float) - cast(@receivedleads_7days as float)) / cast(@receivedleads_7days as float) * 100) as decimal(10, 2)) else cast(0.0 as decimal(10, 2)) end) as receivedp, 

		(case when @totalleads_7days > 0 then cast( ((cast(@totalleads_today as float) - cast(@totalleads_7days as float)) / cast(@totalleads_7days as float) * 100) as decimal(10, 2)) else cast(0.0 as decimal(10, 2)) end) as totalp,
		
		@soldleads_today as sold, 
		(case when @soldleads_7days > 0 then cast( ((cast(@soldleads_today as float) - cast(@soldleads_7days as float)) / cast(@soldleads_7days as float) * 100) as decimal(10, 2)) else cast(0.0 as decimal(10, 2)) end) as soldp,

		@debit_today as debit,
		(case when @debit_7days > 0 then cast( ((cast(@debit_today as float) - cast(@debit_7days as float)) / cast(@debit_7days as float) * 100) as decimal(10, 2)) else cast(0.0 as decimal(10, 2)) end) as debitp,

		@profit_today as profit,
		(case when @profit_7days > 0 then cast( ((cast(@profit_today as float) - cast(@profit_7days as float)) / cast(@profit_7days as float) * 100) as decimal(10, 2)) else cast(0.0 as decimal(10, 2)) end) as profitp
		
		union
		select 3 as num, 'Yesterday (at same time)' as name, @receivedleads_yesterday as received, @totalleads_yesterday as total, cast(0 as decimal(10, 2)) as receivedp, cast(0 as decimal(10, 2)) as totalp, @soldleads_yesterday as sold, cast(0 as decimal(10, 2)) as soldp, @debit_yesterday as debit, cast(0 as decimal(10, 2)) as debitp, @profit_yesterday as profit, cast(0 as decimal(10, 2)) as profitp
		
		union
		select 4 as num, '7 days ago same time' as name, @receivedleads_7days as received, @totalleads_7days as total, cast(0 as decimal(10, 2)) as receivedp, cast(0 as decimal(10, 2)) as totalp, @soldleads_7days as sold, cast(0 as decimal(10, 2)) as soldp, @debit_7days as debit, cast(0 as decimal(10, 2)) as debitp, @profit_7days as profit, cast(0 as decimal(10, 2)) as profitp
	) t
	order by t.num

END