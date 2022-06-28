

-- =============================================
-- Company: Adrack
-- ---------------------------------------------
-- Developer: Zarzand Papikyan
-- Description:	Content Artist Search
-- ---------------------------------------------
-- Execute: EXECUTE [dbo].[usp_membership_user_search]
-- =============================================

CREATE PROCEDURE [dbo].[usp_membership_user_search]
	@Id       BIGINT,
	@Username NVARCHAR(50) = NULL,
	@Email    NVARCHAR(100) = NULL
AS
BEGIN
-- =================================================================================================
-- SQL Script Setup
-- =================================================================================================
	SET NOCOUNT ON

-- =================================================================================================
-- SQL Common
-- =================================================================================================
	DECLARE @SQLCommand        NVARCHAR(4000)
	DECLARE @SQLErrorMessage   NVARCHAR(4000)
	DECLARE @SQLCount          INT
	DECLARE @SQLCounter        INT
	----------------------------------------
	DECLARE @SQLError_Severity INT
	DECLARE @SQLError_State    INT
	DECLARE @SQLError_Message  NVARCHAR(2000)

-- =================================================================================================
-- Process Script
-- =================================================================================================
 	BEGIN TRY
		--------- Search User -------
		SELECT a.[Id],
			   a.[ParentId],
			   a.[UserTypeId],
			   a.[GuId],
			   a.[Username],
			   a.[Email],
			   a.[Password],
			   a.[SaltKey],
			   a.[Active],
			   a.[LockedOut],
			   a.[Deleted],
			   a.[BuiltIn],
			   a.[BuiltInName],
			   a.[RegistrationDate],
			   a.[LoginDate],
			   a.[ActivityDate],
			   a.[PasswordChangedDate],
			   a.[LockoutDate],
			   a.[IpAddress],
			   a.[FailedPasswordAttemptCount],
			   a.[Comment],
			   a.DepartmentId,
			   a.MenuType,
			   a.MaskEmail,
			   a.ValidateOnLogin,
			   a.ChangePassOnLogin,
			   a.TimeZone,
			   a.RemoteLoginGuid,
			   a.ContactEmail
		FROM [dbo].[User] a WITH(NOLOCK)
			INNER JOIN [dbo].[UserType] b WITH(NOLOCK)
				ON a.[UserTypeId] = b.[Id]
			INNER JOIN [dbo].[Profile] c WITH(NOLOCK)
				ON a.[Id] = c.[UserId]
		WHERE a.[Id] = @Id AND
			  b.[Name] = 'Member' OR
			 (a.[Username] LIKE '%' + @Username + '%' OR 
			  a.[Email]    LIKE '%' + @Email + '%')


	END TRY
 
	BEGIN CATCH
		SELECT @SQLError_Severity = ERROR_SEVERITY(), 
		       @SQLError_State    = ERROR_STATE(), 
			   @SQLError_Message  = ERROR_MESSAGE()

		RAISERROR(@SQLError_Message, @SQLError_Severity, @SQLError_State)
	END CATCH
END