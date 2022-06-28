-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ReportTotals]
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@parentid bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @now DateTime = getdate();

	declare @start2 DateTime = @start;
	declare @end2 DateTime = convert(nvarchar(10), cast(@end as Date), 10) + ' ' + cast(cast(@now as Time) as varchar(10));

    -- Insert statements for procedure here

	declare @starttime Time = '00:00:00';
	declare @endtime Time = '23:59:59';

	
	declare @receivedleads_today int = 0;
	declare @totalleads_today int = 0;
	declare @soldleads_today int = 0;
	declare @debit_today money = 0;
	declare @profit_today money = 0;
	declare @redirectedleads_today int = 0;

	select @receivedleads_today = ISNULL(count(Id), 0) from LeadMain where CampaignType = 0 and Created between @start and @end and (@parentid = 0 or (@parentid < 0 and AffiliateId = abs(@parentid))) ;
	select @totalleads_today = ISNULL(count(LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid))) ;
	select @soldleads_today = ISNULL(count(LeadId), 0), @debit_today = ISNULL(sum(BuyerPrice), 0), @profit_today = ISNULL(sum(BuyerPrice - AffiliatePrice), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and Status = 1 and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	select @redirectedleads_today = ISNULL(count(LeadId), 0) from RedirectUrl where Clicked = 1 and Created between @start and @end;
	
	
	----- yesterday -----

	set @start = DATEADD(day, -1, @start2);
	set @end = DATEADD(day, -1, @end2);

	declare @receivedleads_yesterday int = 0;
	declare @totalleads_yesterday int = 0;
	declare @soldleads_yesterday int = 0;
	declare @debit_yesterday money = 0;
	declare @profit_yesterday money = 0;
	declare @redirectedleads_yesterday int = 0;

	select @receivedleads_yesterday = ISNULL(count(distinct LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid))) ;
	select @totalleads_yesterday = ISNULL(count(LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	select @soldleads_yesterday = ISNULL(count(Leadid), 0), @debit_yesterday = ISNULL(sum(BuyerPrice), 0), @profit_yesterday = ISNULL(sum(BuyerPrice - AffiliatePrice), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and Status = 1 and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	select @redirectedleads_yesterday = ISNULL(count(LeadId), 0) from RedirectUrl where Clicked = 1 and Created between @start and @end;	

	----- 7 days -----

	set @start = DATEADD(day, -7, @start2);
	set @end = DATEADD(day, -7, @end2);

	declare @receivedleads_7days int = 0;
	declare @totalleads_7days int = 0;
	declare @soldleads_7days int = 0;
	declare @debit_7days money = 0;
	declare @profit_7days money = 0;
	declare @redirectedleads_7days int = 0;

	select @receivedleads_7days = ISNULL(count(distinct LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	select @totalleads_7days = ISNULL(count(LeadId), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	select @soldleads_7days = ISNULL(count(LeadId), 0), @debit_7days = ISNULL(sum(BuyerPrice), 0), @profit_7days = ISNULL(sum(BuyerPrice - AffiliatePrice), 0) from LeadMainReport where CampaignType = 0 and Created between @start and @end  and Status = 1 and (@parentid = 0 or (@parentid > 0 and BuyerId = @parentid) or (@parentid < 0 and AffiliateId = abs(@parentid)));
	select @redirectedleads_7days = ISNULL(count(LeadId), 0) from RedirectUrl where Clicked = 1 and Created between @start and @end;	
	

	select t.* from (
		select 1 as num, 'Today' as name, @receivedleads_today as received, @totalleads_today as total, cast(0 as money) as receivedp, cast(0 as money) as totalp, @soldleads_today as sold, cast(0 as money) as soldp, @debit_today as debit, cast(0 as money) as debitp, @profit_today as profit, cast(0 as money) as profitp, @redirectedleads_today as redirected, cast(0 as money) as redirectedp
		union
		select 2 as num, 'Yesterday (at same time)' as name, @receivedleads_yesterday as received, @totalleads_yesterday as total, 
		(case when @receivedleads_yesterday > 0 then cast ( ( ( cast(@receivedleads_yesterday as money) - cast(@receivedleads_today as money) ) / cast(@receivedleads_yesterday as money) * 100) as money) else cast(0.0 as money) end) as receivedp, 

		(case when @totalleads_yesterday > 0 then cast( ((cast(@totalleads_yesterday as money) - cast(@totalleads_today as money)) / cast(@totalleads_yesterday as money) * 100) as money) else cast(0.0 as money) end) as totalp,
		
		@soldleads_yesterday as sold, 
		(case when @soldleads_yesterday > 0 then cast( ((cast(@soldleads_yesterday as money) - cast(@soldleads_today as money)) / cast(@soldleads_yesterday as money) * 100) as money) else cast(0.0 as money) end) as soldp,

		@debit_yesterday as debit,
		(case when @debit_yesterday > 0 then cast( ((cast(@debit_yesterday as money) - cast(@debit_today as money)) / cast(@debit_yesterday as money) * 100) as money) else cast(0.0 as money) end) as debitp,

		@profit_yesterday as profit,
		(case when @profit_yesterday > 0 then cast( ((cast(@profit_yesterday as money) - cast(@profit_today as money)) / cast(@profit_yesterday as money) * 100) as money) else cast(0.0 as money) end) as profitp,

		@redirectedleads_yesterday as redirected,
		(case when @redirectedleads_yesterday > 0 then cast( ((cast(@redirectedleads_yesterday as money) - cast(@redirectedleads_today as money)) / cast(@redirectedleads_yesterday as money) * 100) as money) else cast(0.0 as money) end) as redirectedp

		union
		select 3 as num, '7 days ago same time' as name, @receivedleads_7days as received, @totalleads_7days as total, 
		(case when @receivedleads_7days > 0 then cast ( ((cast(@receivedleads_7days as money) - cast(@receivedleads_today as money)) / cast(@receivedleads_7days as money) * 100) as money) else cast(0.0 as money) end) as receivedp, 

		(case when @totalleads_7days > 0 then cast( ((cast(@totalleads_7days as money) - cast(@totalleads_today as money)) / cast(@totalleads_7days as money) * 100) as money) else cast(0.0 as money) end) as totalp,
		
		@soldleads_7days as sold, 
		(case when @soldleads_7days > 0 then cast( ((cast(@soldleads_7days as money) - cast(@soldleads_today as money)) / cast(@soldleads_7days as money) * 100) as money) else cast(0.0 as money) end) as soldp,

		@debit_7days as debit,
		(case when @debit_7days > 0 then cast( ((cast(@debit_7days as money) - cast(@debit_today as money)) / cast(@debit_7days as money) * 100) as money) else cast(0.0 as money) end) as debitp,

		@profit_7days as profit,
		(case when @profit_7days > 0 then cast( ((cast(@profit_7days as money) - cast(@profit_today as money)) / cast(@profit_7days as money) * 100) as money) else cast(0.0 as money) end) as profitp,

		@redirectedleads_7days as redirected,
		(case when @redirectedleads_7days > 0 then cast( (cast(@redirectedleads_7days as money) - (cast(@redirectedleads_today as money)) / cast(@redirectedleads_7days as money) * 100) as money) else cast(0.0 as money) end) as redirectedp
	) t
	order by t.num

END
