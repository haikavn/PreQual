-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSupportTicketsMessages]
	-- Add the parameters for the stored procedure here
	@TicketId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @NewCount int = 0;
	SELECT @NewCount = Count(SupportTicketsMessages.IsNew)
	FROM SupportTicketsMessages
	WHERE SupportTicketsMessages.TicketID = @TicketId;


	SELECT SupportTicketsMessages.*, [User].Username AS Username, @NewCount AS NewCount
	FROM SupportTicketsMessages
	INNER JOIN [User] ON [User].Id = SupportTicketsMessages.AuthorID
	WHERE SupportTicketsMessages.TicketID = @TicketId
	ORDER BY SupportTicketsMessages.Id ASC
END