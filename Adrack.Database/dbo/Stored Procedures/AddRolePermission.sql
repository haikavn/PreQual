-- =============================================
-- Company: Adrack.com
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Add Role Permission
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[AddRolePermission]
-- =============================================

Create PROCEDURE [dbo].[AddRolePermission]
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
		IF NOT EXISTS(SELECT 1 FROM [dbo].[RolePermission] WHERE [RoleId] = @RoleId AND [PermissionId] = @PermissionId)
			BEGIN
				INSERT INTO [dbo].[RolePermission] (RoleId, PermissionId, [State])
				VALUES (@RoleId, @PermissionId, @State);
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