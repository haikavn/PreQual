-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description: DisableBuyerInvoice>
-- =============================================
CREATE PROCEDURE [dbo].[DisableBuyerInvoice]
	@InvoiceId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE BuyerInvoice
	SET [Status] = -1
	WHERE Id = @InvoiceId;

	UPDATE LeadMainReport
	SET BInvoiceId = NULL
	WHERE BInvoiceId = @InvoiceId;

	SELECT @InvoiceId;

END