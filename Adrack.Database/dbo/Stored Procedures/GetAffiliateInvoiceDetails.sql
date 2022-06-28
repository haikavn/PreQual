
-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAffiliateInvoiceDetails]
	@InvoiceID bigint	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT LeadMainReport.AffiliatePrice AS AffiliatePrice, count(LeadMainReport.Id) AS AffiliateLeadsCount, sum(LeadMainReport.AffiliatePrice) as AffiliateSum, LeadMainReport.CampaignId AS CampaignId, Campaign.Name AS CampaignName
	FROM LeadMainReport
	INNER JOIN Campaign on Campaign.Id = LeadMainReport.CampaignId
	WHERE LeadMainReport.AInvoiceId = @InvoiceID AND LeadMainReport.[Status] = 1
	GROUP BY LeadMainReport.AffiliatePrice, LeadMainReport.CampaignId, Campaign.Name
END