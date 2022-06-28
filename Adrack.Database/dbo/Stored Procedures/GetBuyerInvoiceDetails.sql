-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetBuyerInvoiceDetails]
	@InvoiceID bigint	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT LeadMainReport.BuyerPrice AS BuyerPrice, count(LeadMainReport.Id) AS BuyerLeadsCount, sum(LeadMainReport.BuyerPrice) as BuyerSum, LeadMainReport.CampaignId AS CampaignId, Campaign.Name AS CampaignName
	FROM dbo.LeadMainReport
	INNER JOIN dbo.Campaign on Campaign.Id = LeadMainReport.CampaignId
	WHERE LeadMainReport.BInvoiceId = @InvoiceID AND LeadMainReport.[Status] = 1
	GROUP BY LeadMainReport.BuyerPrice, LeadMainReport.CampaignId, Campaign.Name
END