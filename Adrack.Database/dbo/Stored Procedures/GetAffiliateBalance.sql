

-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAffiliateBalance]
	@AffiliateId bigint,
	@DateFrom datetime,
	@DateTo datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT t.*, CAST(t.PaymentSum - t.InvoicedSum AS money) AS Balance FROM (
SELECT
	Affiliate.Id AS BuyerId,
	
	Affiliate.Name, Affiliate.Email,
	
	(SELECT CAST(ISNULL(SUM(LeadMainReportDay.AffiliatePrice),0) AS money) FROM dbo.LeadMainReportDay WHERE dbo.LeadMainReportDay.[Status]=1 AND dbo.LeadMainReportDay.AffiliateId = Affiliate.Id AND ( (@DateFrom IS NULL AND @DateTo IS NULL) OR LeadMainReportDay.Created between @DateFrom AND @DateTo)) AS SoldSum,
	
	(SELECT CAST( ISNULL(SUM(AffiliateInvoice.Paid),0) AS money)
	FROM dbo.AffiliateInvoice
	WHERE AffiliateInvoice.AffiliateId = Affiliate.Id AND ((@DateFrom IS NULL AND @DateTo IS NULL) OR AffiliateInvoice.DateCreated between @DateFrom AND @DateTo ) ) AS PaymentSum,
	
	(SELECT CAST(ISNULL( SUM(AffiliateInvoice.[Sum] - AffiliateInvoice.Refunded + AffiliateInvoice.Adjustment), 0) AS money) 
	FROM dbo.AffiliateInvoice
	WHERE AffiliateInvoice.AffiliateId = Affiliate.Id AND AffiliateInvoice.[Status] >= 1 AND ((@DateFrom IS NULL AND @DateTo IS NULL) OR AffiliateInvoice.DateCreated between @DateFrom AND @DateTo) ) AS InvoicedSum,

	CAST( 0 AS money) AS Credit
	FROM dbo.Affiliate

WHERE (@AffiliateId = 0 OR Affiliate.Id = @AffiliateId)
GROUP BY Affiliate.Id, Affiliate.[Name], Affiliate.Email
) AS t

END