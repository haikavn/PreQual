
-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ReportByStatuses]
	-- Add the parameters for the stored procedure here
	@StartDate DateTime,
	@EndDate DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT CAST([Status] AS bigint) AS [Status], CAST(Count(Id) AS bigint) AS Counts
	FROM [dbo].[LeadMainReportDay]
	WHERE Created BETWEEN @StartDate AND @EndDate
	GROUP BY [Status]

  
END