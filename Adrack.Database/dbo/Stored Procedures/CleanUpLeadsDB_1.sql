-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CleanUpLeadsDB]
	@password varchar(32)
	
AS
BEGIN
	SET NOCOUNT ON;

	IF (@password = 'yes')
	BEGIN
		DELETE FROM [dbo].[AffiliateResponse];
		DBCC CHECKIDENT('AffiliateResponse', RESEED, 0);

		DELETE FROM [dbo].[LeadContent];
		DBCC CHECKIDENT('LeadContent', RESEED, 0);

		DELETE FROM [dbo].[LeadContentDublicate];
		DBCC CHECKIDENT('LeadContentDublicate', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDay];
		DBCC CHECKIDENT('LeadMainReportDay', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDayAffiliate];
		DBCC CHECKIDENT('LeadMainReportDayAffiliate', RESEED, 0);
	
		DELETE FROM [dbo].[LeadMainReportDayReceived];
		DBCC CHECKIDENT('LeadMainReportDayReceived', RESEED, 0);		

		DELETE FROM [dbo].[LeadMainReportDayRedirected];
		DBCC CHECKIDENT('LeadMainReportDayRedirected', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDayPrices];
		DBCC CHECKIDENT('LeadMainReportDayPrices', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDayHour];
		DBCC CHECKIDENT('LeadMainReportDayHour', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportTotalDay];
		DBCC CHECKIDENT('LeadMainReportTotalDay', RESEED, 0);

		DELETE FROM [dbo].[LeadMainResponse];
		DBCC CHECKIDENT('LeadMainResponse', RESEED, 0);

		DELETE FROM [dbo].[BuyerResponse];
		DBCC CHECKIDENT('BuyerResponse', RESEED, 0);

		DELETE FROM [dbo].[PostedData];
		DBCC CHECKIDENT('PostedData', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReport];
		DBCC CHECKIDENT('LeadMainReport', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDaySubIds];
		DBCC CHECKIDENT('LeadMainReportDaySubIds', RESEED, 0);

		DELETE FROM [dbo].[LeadSensitiveData];
		DBCC CHECKIDENT('LeadSensitiveData', RESEED, 0);

		DELETE FROM [dbo].[LeadMain];
		DBCC CHECKIDENT('LeadMain', RESEED, 0);
	END
	ELSE
	BEGIN
		SELECT 'Error'
	END

END