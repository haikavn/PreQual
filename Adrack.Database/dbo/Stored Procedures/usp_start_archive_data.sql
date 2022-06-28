-- =============================================
-- Developer: Zarzand Papikyan
-- Description:	Start Archive Data
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[usp_start_archive_data]
-- =============================================
CREATE PROCEDURE [dbo].[usp_start_archive_data]
AS
BEGIN
-- =================================================================================================
-- SQL Script Setup
-- =================================================================================================
	SET NOCOUNT    ON
	SET XACT_ABORT ON

-- =================================================================================================
-- SQL Common
-- =================================================================================================
	DECLARE @SQLCommand                      NVARCHAR(MAX)
	DECLARE @SQLCommandType                  NVARCHAR(MAX)
	DECLARE @SQLCommandOutput                NVARCHAR(MAX)
	DECLARE @SQLCharacter                    CHAR(3)
	----------------------------------------
	DECLARE @SQLCount                        INT = 0
	DECLARE @SQLCounter                      INT = 1
	----------------------------------------
	DECLARE @SQLError_Severity               INT
	DECLARE @SQLError_State                  INT = 0
	DECLARE @SQLError_Message                NVARCHAR(MAX)
	----------------------------------------

-- =================================================================================================
-- Drop SQL Temp Tables
-- =================================================================================================

-- =================================================================================================
-- SQL Temp Tables
-- =================================================================================================

-- =================================================================================================
-- Validation
-- =================================================================================================

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY
		EXECUTE [dbo].[usp_get_archive_data] @TableName = N'LeadMain'
	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()
		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END