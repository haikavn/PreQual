-- =============================================
-- Author:		<Author,,Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetResponsesByLeadId]
	-- Add the parameters for the stored procedure here
	@leadId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
SELECT DISTINCT LeadMainResponse.Id, 
LeadMainResponse.BuyerId, 
Buyer.Name AS BuyerName, 
LeadMainResponse.BuyerChannelId, 
LeadMainResponse.LeadID,
BuyerChannel.Name AS BuyerChanelName, 
LeadMainResponse.Response, 
LeadMainResponse.ResponseTime, LeadMainResponse.[Status], LeadMainResponse.MinPrice, 
PostedData.Posted, PostedData.Created, LeadMainResponse.Created AS ResponseCreated

FROM LeadMainResponse

INNER JOIN Buyer ON LeadMainResponse.BuyerId=Buyer.Id
INNER JOIN BuyerChannel ON LeadMainResponse.BuyerChannelId=BuyerChannel.Id
 INNER JOIN PostedData ON PostedData.LeadId=LeadMainResponse.LeadId AND 
 PostedData.MinPrice=LeadMainResponse.MinPrice AND 
 PostedData.BuyerChannelId = LeadMainResponse.BuyerChannelId and (PostedData.[Status] is null or (PostedData.[Status] is not null and PostedData.[Status] = LeadMainResponse.[Status]))
WHERE  LeadMainResponse.LeadId = @leadId  ORDER By LeadMainResponse.ID DESC
	
END