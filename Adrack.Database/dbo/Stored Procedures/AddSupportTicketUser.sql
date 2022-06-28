-- =============================================
-- Company: Adrack.com
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Add Support TicketUser
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[AddSupportTicketUser]
-- =============================================

CREATE PROCEDURE [dbo].[AddSupportTicketUser]
	@TicketId BIGINT,
	@UserId   BIGINT
AS
BEGIN
-- =================================================================================================
-- SQL Script Setup
-- =================================================================================================
	SET NOCOUNT ON

-- =================================================================================================
-- SQL Common
-- =================================================================================================
	DECLARE @SQLError_Severity INT
	DECLARE @SQLError_State    INT
	DECLARE @SQLError_Message  NVARCHAR(2000)

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY
		INSERT INTO [dbo].[SupportTicketsUser] ([TicketID], [UserID])
		VALUES(@TicketId, @UserId);

		select cast(SCOPE_IDENTITY() as bigint);
	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END