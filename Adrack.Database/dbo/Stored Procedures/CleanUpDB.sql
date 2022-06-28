-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CleanUpDB]
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

		DELETE FROM [dbo].[LeadMainResponse];
		DBCC CHECKIDENT('LeadMainResponse', RESEED, 0);

		DELETE FROM [dbo].[BuyerResponse];
		DBCC CHECKIDENT('BuyerResponse', RESEED, 0);

		DELETE FROM [dbo].[PostedData];
		DBCC CHECKIDENT('PostedData', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReport];
		DBCC CHECKIDENT('LeadMainReport', RESEED, 0);

		DELETE FROM [dbo].[LeadFieldsContent];
		DBCC CHECKIDENT('LeadFieldsContent', RESEED, 0);

		DELETE FROM [dbo].[LeadMainReportDaySubIds];
		DBCC CHECKIDENT('LeadMainReportDaySubIds', RESEED, 0);

		DELETE FROM [dbo].[LeadSensitiveData];
		DBCC CHECKIDENT('LeadSensitiveData', RESEED, 0);

		DELETE FROM [dbo].[LeadMain];
		DBCC CHECKIDENT('LeadMain', RESEED, 0);

		DELETE FROM [dbo].[Log];
		DBCC CHECKIDENT('Log', RESEED, 0);


		DELETE FROM [dbo].[AffiliateChannelFilterCondition];
		DBCC CHECKIDENT('AffiliateChannelFilterCondition', RESEED, 0);

		DELETE FROM [dbo].[AffiliateChannelTemplate];
		DBCC CHECKIDENT('AffiliateChannelTemplate', RESEED, 0);

		DELETE FROM [dbo].[AffiliateChannel];
		DBCC CHECKIDENT('AffiliateChannel', RESEED, 0);

		DELETE FROM [dbo].[AffiliateNote];
		DBCC CHECKIDENT('AffiliateNote', RESEED, 0);

		DELETE FROM [dbo].[Affiliate];
		DBCC CHECKIDENT('Affiliate', RESEED, 0);


		DELETE FROM [dbo].[BuyerChannelSchedule];
		DBCC CHECKIDENT('BuyerChannelSchedule', RESEED, 0);

		DELETE FROM [dbo].[BuyerChannelTemplate];
		DBCC CHECKIDENT('BuyerChannelTemplate', RESEED, 0);

		DELETE FROM [dbo].[BuyerChannelFilterCondition];
		DBCC CHECKIDENT('BuyerChannelFilterCondition', RESEED, 0);

		DELETE FROM [dbo].[BuyerChannelTemplateMatching];
		DBCC CHECKIDENT('BuyerChannelTemplateMatching', RESEED, 0);		

		DELETE FROM [dbo].[BuyerChannel];
		DBCC CHECKIDENT('BuyerChannel', RESEED, 0);

		DELETE FROM [dbo].[Buyer];
		DBCC CHECKIDENT('Buyer', RESEED, 0);

		DELETE FROM [dbo].[History];
		DBCC CHECKIDENT('History', RESEED, 0);

		DELETE FROM [dbo].[EmailQueue];
		DBCC CHECKIDENT('EmailQueue', RESEED, 0);

		DELETE FROM [dbo].[LeadNote];
		DBCC CHECKIDENT('LeadNote', RESEED, 0);

		DELETE FROM [dbo].[Log];
		DBCC CHECKIDENT('Log', RESEED, 0);

		DELETE FROM [dbo].[PaymentMethod];
		DBCC CHECKIDENT('PaymentMethod', RESEED, 0);

		DELETE FROM [dbo].[SupportTicketsUser];
		DBCC CHECKIDENT('PaymentMethod', RESEED, 0);

		DELETE FROM [dbo].[SupportTicketsMessages];
		DBCC CHECKIDENT('PaymentMethod', RESEED, 0);
		
		DELETE FROM [dbo].[SupportTicketsUser];
		DBCC CHECKIDENT('PaymentMethod', RESEED, 0);

		DELETE FROM [dbo].[VerifyAccount];
		DBCC CHECKIDENT('VerifyAccount', RESEED, 0);

		DELETE FROM [dbo].[UserRole]
		WHERE UserId != 1

		DELETE FROM [dbo].[Profile]
		WHERE UserId != 1
		DBCC CHECKIDENT('Profile', RESEED, 1);

		DELETE FROM [dbo].[User]
		WHERE UserTypeId != 1
		DBCC CHECKIDENT('User', RESEED, 1);

		DELETE FROM [dbo].[Vertical]
		WHERE Id != 1;
		DBCC CHECKIDENT('Vertical', RESEED, 1);

		DELETE FROM [dbo].[AffiliateInvoice];
		DBCC CHECKIDENT('AffiliateInvoice', RESEED, 0);

		DELETE FROM [dbo].[AffiliateInvoiceAdjustment];
		DBCC CHECKIDENT('AffiliateInvoiceAdjustment', RESEED, 0);

		DELETE FROM [dbo].[AffiliateNote];
		DBCC CHECKIDENT('AffiliateNote', RESEED, 0);

		DELETE FROM [dbo].[AffiliatePayment];
		DBCC CHECKIDENT('AffiliatePayment', RESEED, 0);

		DELETE FROM [dbo].[BlackListValue];
		DBCC CHECKIDENT('BlackListValue', RESEED, 0);
		
		DELETE FROM [dbo].[BuyerBalance];
		DBCC CHECKIDENT('BuyerBalance', RESEED, 0);
		
		DELETE FROM [dbo].[BuyerInvoiceAdjustment];
		DBCC CHECKIDENT('BuyerInvoiceAdjustment', RESEED, 0);

		DELETE FROM [dbo].[BuyerInvoice];
		DBCC CHECKIDENT('BuyerInvoice', RESEED, 0);

		DELETE FROM [dbo].[BuyerPayment];
		DBCC CHECKIDENT('BuyerPayment', RESEED, 0);

		DELETE FROM [dbo].[RedirectUrl];
		DBCC CHECKIDENT('RedirectUrl', RESEED, 0);

		DELETE FROM [dbo].[RefundedLeads];
		DBCC CHECKIDENT('RefundedLeads', RESEED, 0);
		
		DELETE FROM [dbo].[CampaignTemplate];
		DBCC CHECKIDENT('CampaignTemplate', RESEED, 0);

		DELETE FROM [dbo].[Filter];
		DBCC CHECKIDENT('Filter', RESEED, 0);

		DELETE FROM [dbo].[Campaign];
		DBCC CHECKIDENT('Campaign', RESEED, 0);
	END
	ELSE
	BEGIN
		SELECT 'Error'
	END

END