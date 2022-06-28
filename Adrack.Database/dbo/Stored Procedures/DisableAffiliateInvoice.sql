-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description: DisableAffiliateInvoice>
-- =============================================
CREATE PROCEDURE [dbo].[DisableAffiliateInvoice]
	@InvoiceId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE AffiliateInvoice
	SET [Status] = -1
	WHERE Id = @InvoiceId;

	UPDATE LeadMainReport
	SET AInvoiceId = NULL
	WHERE AInvoiceId = @InvoiceId;

	SELECT @InvoiceId;

END