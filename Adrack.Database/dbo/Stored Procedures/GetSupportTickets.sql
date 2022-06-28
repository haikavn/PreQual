-- =============================================
-- Author:		Arman Zakaryan
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSupportTickets]
	-- Add the parameters for the stored procedure here
	@UserId bigint,
	@UserIds varchar(MAX) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	SELECT SupportTickets.*,
	(SELECT [User].Username FROM [User] WHERE [User].Id = SupportTickets.UserID) AS Username,
	(SELECT [User].Username FROM [User] WHERE [User].Id = SupportTickets.ManagerID) AS Managername,
	(SELECT count(SupportTicketsMessages.IsNew) FROM SupportTicketsMessages WHERE SupportTicketsMessages.TicketID=SupportTickets.Id AND SupportTicketsMessages.IsNew=1) AS NewCount
	FROM SupportTickets
	WHERE @UserId = 0 OR (SupportTickets.ManagerID = @UserId OR SupportTickets.UserID = @UserId) or (len(@UserIds) > 0 and SupportTickets.[UserID] in (select Item from dbo.SplitInts(@UserIds, ',')))
	ORDER BY SupportTickets.Id DESC
END