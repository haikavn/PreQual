-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAllAffiliatePayments]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT AffiliatePayment.[Id], AffiliatePayment.[AffiliateId], AffiliatePayment.[PaymentDate], AffiliatePayment.[Amount], AffiliatePayment.[Note], AffiliatePayment.[Created], AffiliatePayment.[UserId], Affiliate.Name AS AffiliateName, [User].Username AS UserName
	FROM dbo.AffiliatePayment
	INNER JOIN dbo.Affiliate on AffiliatePayment.AffiliateId=Affiliate.Id
	LEFT JOIN dbo.[User] on AffiliatePayment.UserId = [User].Id
END