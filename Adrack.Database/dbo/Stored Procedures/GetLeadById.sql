
-- =============================================
-- Author:		<Author,,Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetLeadById]
	-- Add the parameters for the stored procedure here
	@leadId bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT LeadMain.*, LeadContent.*, LeadMainResponse.BuyerId, LeadMainResponse.BuyerChannelId, LeadMainResponse.AffiliatePrice, LeadMainResponse.BuyerPrice
	FROM LeadMain
	INNER JOIN LeadContent ON LeadMain.Id = LeadContent.LeadId
	LEFT JOIN LeadMainResponse ON LeadMain.Id = LeadMainResponse.LeadId AND LeadMainResponse.Status=1 
	WHERE LeadMain.Id=@leadId
	ORDER BY LeadMainResponse.Id Desc
END