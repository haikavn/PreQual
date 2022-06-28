-- =============================================
-- Developer: Zarzand Papikyan
-- Description:	Archiving A Table
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[usp_get_archive_data]
-- =============================================
CREATE PROCEDURE [dbo].[usp_get_archive_data]
	@StartDate      DATETIME = NULL,
	@EndDate        DATETIME = NULL,
	@TableName      VARCHAR(100),
	@BatchSize      INT = 1000,
	@DWTableName    VARCHAR(100) = NULL  OUTPUT,
	@DWTableSchema  VARCHAR(MAX) = NULL OUTPUT
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
	-- Current Date
	IF(@StartDate IS NULL)
		BEGIN
			SET @StartDate = CONVERT(VARCHAR(10), GETDATE(), 120)
		END

	-- Six Months Back From Today
	IF(@EndDate IS NULL)
		BEGIN
			SET @EndDate = CONVERT(VARCHAR(10), GETDATE() - 6, 120)
		END

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY
		SET @SQLCommandOutput = 'SELECT TOP(1) * INTO [tempdb].[dbo].[#SchemaOutput] FROM [dbo].[' + @TableName + '] WITH(NOLOCK)'
		EXECUTE [sp_executesql] @SQLCommandOutput

		SET @SQLCommand = 'SELECT TOP(' + CAST(@BatchSize AS VARCHAR) + ') * FROM [dbo].[' + @TableName + '] WITH(NOLOCK)' + ' WHERE [CreateArchiveDT] NOT BETWEEN ''' + CONVERT(VARCHAR(10), @EndDate, 120) + ''' AND ''' + CONVERT(VARCHAR(10), @StartDate, 120) + ''''
		
		SET @DWTableName = @TableName
		
		SET @DWTableSchema =
			'SET @DWTableSchema = ' +
				'''CREATE TABLE [dbo].[' + @TableName + '] ( '' + ' +
					'STUFF ' +
					'( ' +
						'( ' +
							'SELECT ' +
								''','' + ' +
								'QUOTENAME(COLUMN_NAME) + '' '' + ' +
								'DATA_TYPE + ' + 
								'CASE ' +
									'WHEN DATA_TYPE LIKE ''%char'' THEN ''('' + COALESCE(NULLIF(CONVERT(VARCHAR, CHARACTER_MAXIMUM_LENGTH), ''-1''), ''max'') + '') '' ' +
									'ELSE '' '' ' +
								'END + ' +
								'CASE IS_NULLABLE ' +
									'WHEN ''NO'' THEN ''NOT '' ' +
									'ELSE '''' ' +
								'END + ''NULL'' AS [text()] ' +
							'FROM [tempdb].[INFORMATION_SCHEMA].[COLUMNS] ' +
							'WHERE ' +
								'TABLE_NAME = (SELECT name FROM tempdb.sys.objects WHERE object_id = OBJECT_ID(''[tempdb].[dbo].[#SchemaOutput]'')) ' +
								'ORDER BY ' +
									'ORDINAL_POSITION ' +
							'FOR XML ' +
								'PATH('''') ' +
						'), + ' +
						'1, ' +
						'1, ' +
						''''' ' +
					') + ' +
				''')''; ' 
		EXECUTE [sp_executesql] @DWTableSchema, N'@DWTableSchema VARCHAR(MAX) OUTPUT', @DWTableSchema OUTPUT
		EXECUTE [sp_executesql] @SQLCommand
	END TRY

	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
				@SQLError_State    = ERROR_STATE(), 
				@SQLError_Message  = ERROR_MESSAGE()
		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END