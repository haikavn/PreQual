-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ReportByTopStates] 
	@StartDate DateTime,
	@EndDate DateTime,
	@Count int,
	@BuyerId bigint,
	@AffiliateId bigint,
	@CampaingnId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT TOP (@Count) CAST(SUM(Quantity) AS bigint) AS Counts, [State]
	FROM [dbo].[LeadMainReportDay]
	WHERE Created BETWEEN @StartDate AND @EndDate AND (@BuyerId = 0 OR [dbo].[LeadMainReportDay].BuyerId = @BuyerId) AND (@AffiliateId = 0 OR [dbo].[LeadMainReportDay].AffiliateId = @AffiliateId) AND (@CampaingnId = 0 OR [dbo].[LeadMainReportDay].CampaignId = @CampaingnId)
	GROUP BY [State]
	ORDER BY Counts DESC

END