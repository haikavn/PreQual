-- =============================================
-- Author:		Arman Zakaryan
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SetTicketMessagesRead] 
	-- Add the parameters for the stored procedure here
	@ticketId bigint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

UPDATE SupportTicketsMessages
SET SupportTicketsMessages.IsNew = 0
WHERE SupportTicketsMessages.TicketID = @ticketId AND SupportTicketsMessages.IsNew = 1;

SELECT 1
END