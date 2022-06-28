CREATE PROCEDURE [dbo].[SalesReport] 
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@type varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	if @type = 'price'
	begin
		select cast(BuyerPrice as varchar(50)) as [Name], sum(quantity) as [Value] from LeadMainReportDayPrices where [status] = 1 and created between @start and @end group by BuyerPrice order by BuyerPrice;
	end;

	if @type = 'dayofweek'
	begin
		select cast(DATEPART(weekday, created) as varchar(50)) as [Name], sum(quantity) as [Value] from LeadMainReportDay where [status] = 1 and created between @start and @end group by DATEPART(weekday, created) order by DATEPART(weekday, created);
	end;

	if @type = 'hour'
	begin
		select cast([hour] as varchar(50)) as [Name], sum(quantity) as [Value] from LeadMainReportDayHour where [status] = 1 and created between @start and @end group by [hour] order by [hour];
	end;

	if @type = 'state'
	begin
		select cast([state] as varchar(50)) as [Name], sum(quantity) as [Value] from LeadMainReportDay where [status] = 1 and created between @start and @end group by [state] order by [state];
	end;

	--select 0 as rownum, 0 as id, '' as name, 0 as fid, 0 as totalleads, 0 as soldleads, cast(0 as money) as debet, cast(0 as money) as credit;
END