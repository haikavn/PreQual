-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ClearCache]
	
AS
BEGIN
	SET NOCOUNT ON;

		DELETE FROM [dbo].[LeadMainReportDay];
		DBCC CHECKIDENT('LeadMainReportDay', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDayHour];
		DBCC CHECKIDENT('LeadMainReportDayHour', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDayAffiliate];
		DBCC CHECKIDENT('LeadMainReportDayAffiliate', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDayReceived];
		DBCC CHECKIDENT('LeadMainReportDayReceived', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDayRedirected];
		DBCC CHECKIDENT('LeadMainReportDayRedirected', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDayPrices];
		DBCC CHECKIDENT('LeadMainReportDayPrices', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDaySubIds];
		DBCC CHECKIDENT('LeadMainReportDaySubIds', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReport];
		DBCC CHECKIDENT('LeadMainReport', RESEED, 0);

		exec dbo.FillMainReport;
END