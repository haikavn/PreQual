-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetBuyersBalance]
	@BuyerId bigint,
	@DateFrom datetime,
	@DateTo datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT t.*, CAST(t.PaymentSum - t.InvoicedSum AS money) AS Balance FROM (
SELECT Buyer.Id AS BuyerId, Buyer.Name, Buyer.Email, --CAST(ISNULL(SUM(LeadMainReportDay.BuyerPrice),0) AS money) AS SoldSum,
(SELECT CAST(ISNULL(SUM(LeadMainReportDay.BuyerPrice),0) AS money) FROM dbo.LeadMainReportDay WHERE dbo.LeadMainReportDay.[Status]=1 AND dbo.LeadMainReportDay.BuyerId = Buyer.Id AND ( (@DateFrom IS NULL AND @DateTo IS NULL) OR LeadMainReportDay.Created between @DateFrom AND @DateTo)) AS SoldSum,
 (SELECT CAST( ISNULL(SUM(BuyerPayment.Amount),0) AS money)
FROM dbo.BuyerPayment
WHERE BuyerPayment.BuyerId = Buyer.Id AND ((@DateFrom IS NULL AND @DateTo IS NULL) OR BuyerPayment.Created between @DateFrom AND @DateTo ) ) AS PaymentSum,
(SELECT CAST(ISNULL( SUM(BuyerInvoice.[Sum] - BuyerInvoice.Refunded + BuyerInvoice.Adjustment), 0) AS money) 
FROM dbo.BuyerInvoice
WHERE BuyerInvoice.BuyerId = Buyer.Id AND BuyerInvoice.[Status] >= 1 AND ((@DateFrom IS NULL AND @DateTo IS NULL) OR BuyerInvoice.DateCreated between @DateFrom AND @DateTo) ) AS InvoicedSum,
CAST( ISNULL((SELECT Credit FROM BuyerBalance WHERE BuyerBalance.BuyerId = Buyer.Id),0) AS money) AS Credit
FROM dbo.Buyer
--LEFT JOIN dbo.LeadMainReportDay ON LeadMainReportDay.BuyerId = Buyer.Id AND LeadMainReportDay.[Status] = 1
WHERE (@BuyerId = 0 OR Buyer.Id = @BuyerId) /* AND ( (@DateFrom IS NULL AND @DateTo IS NULL) OR LeadMainReportDay.Created between @DateFrom AND @DateTo) */
GROUP BY Buyer.Id, Buyer.Name, Buyer.Email
) AS t

END