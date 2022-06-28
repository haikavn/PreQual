-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetBuyerDistrib]
	@BuyerId bigint
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @InvoiceSum float = 0;
	DECLARE @PaymentAmount float = 0;
	DECLARE @InvoicePaid float = 0;

	SELECT @InvoiceSum = SUM([Sum]), @InvoicePaid = SUM(Paid)
	FROM dbo.BuyerInvoice
	WHERE BuyerId = @BuyerId AND [Status] != -1;

	SELECT @PaymentAmount = SUM(Amount)
	FROM dbo.BuyerPayment
	WHERE BuyerId = @BuyerId;

	IF (@PaymentAmount IS NULL OR @InvoicePaid IS NULL)
	BEGIN
		SELECT CAST(0 AS float)
	END
	ELSE
	BEGIN
		SELECT CAST(@PaymentAmount - @InvoicePaid AS float) AS Distrib
	END
END