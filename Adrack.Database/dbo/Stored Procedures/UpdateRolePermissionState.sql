Create PROCEDURE [dbo].[UpdateRolePermissionState]
	@RoleId       BIGINT,
	@PermissionId BIGINT,
	@State TINYINT
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
		
				Update [dbo].[RolePermission] 
				Set  [State]= @State
				WHERE RoleId = @RoleId and PermissionId = @PermissionId

				SELECT CAST(0 AS INT);
	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END




