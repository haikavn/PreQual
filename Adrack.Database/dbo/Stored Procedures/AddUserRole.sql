CREATE PROCEDURE [dbo].[AddUserRole]
	@RoleId       BIGINT,
	@UserId BIGINT

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
		IF NOT EXISTS(SELECT 1 FROM [dbo].[UserRole] WHERE [RoleId] = @RoleId AND [UserId] = @UserId)
			BEGIN
				INSERT INTO [dbo].[UserRole] (UserId, RoleId)
				VALUES (@UserId, @RoleId);
			END

		SELECT 0;

	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END
