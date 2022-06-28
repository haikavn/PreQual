
-- =============================================
-- Author:		Arman Zakaryan
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSupportTicketsByUsers]
	@ParentId bigint,
	@Type int
AS
BEGIN

	SET NOCOUNT ON;

    
SELECT SupportTickets.*,
	(SELECT [User].Username FROM [User] WHERE [User].Id = SupportTickets.UserID) AS Username,
	(SELECT [User].Username FROM [User] WHERE [User].Id = SupportTickets.ManagerID) AS Managername,
	(SELECT count(SupportTicketsMessages.IsNew) FROM SupportTicketsMessages WHERE SupportTicketsMessages.TicketID=SupportTickets.Id AND SupportTicketsMessages.IsNew=1) AS NewCount
	FROM SupportTickets
	WHERE  (SupportTickets.ManagerID in (
										SELECT [User].Id
										FROM [User]
										WHERE ([User].UserTypeId = @Type AND [User].ParentId=@ParentId)
										)
									OR SupportTickets.UserID in (
										SELECT [User].Id
										FROM [User]
										WHERE ([User].UserTypeId = @Type AND [User].ParentId=@ParentId)
										))
	ORDER BY SupportTickets.Id DESC
END