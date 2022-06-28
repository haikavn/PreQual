CREATE PROCEDURE [dbo].[ReportByHour]
	-- Add the parameters for the stored procedure here
	@start DateTime,
	@end DateTime,
	@parentid bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select t.* from (SELECT 'sold' as activity, count (distinct[LeadId]) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, DATEPART(DAY, Created) as dy, DATEPART(HOUR, Created) as hr  
	FROM [dbo].[LeadMainReport] where Created between @start and @end and Status=1 group by DATEPART(YEAR, Created), DATEPART(MONTH, Created), DATEPART(DAY, Created), DATEPART(HOUR, Created)
	union
	SELECT 'received' as activity, count (distinct[LeadId]) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, DATEPART(DAY, Created) as dy, DATEPART(HOUR, Created) as hr  
	FROM [dbo].[LeadMainReport] where Created between @start and @end group by DATEPART(YEAR, Created), DATEPART(MONTH, Created), DATEPART(DAY, Created), DATEPART(HOUR, Created)		
	union
	SELECT 'posted' as activity, count ([LeadId]) as leads, DATEPART(YEAR, Created) as yr, DATEPART(MONTH, Created) as mt, DATEPART(DAY, Created) as dy, DATEPART(HOUR, Created) as hr  
	FROM [dbo].[LeadMainReport] where Created between @start and @end group by DATEPART(YEAR, Created), DATEPART(MONTH, Created), DATEPART(DAY, Created), DATEPART(HOUR, Created)) t order by t.yr, t.mt, t.dy, t.hr

END