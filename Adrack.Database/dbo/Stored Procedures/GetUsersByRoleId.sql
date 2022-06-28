-- =============================================
-- Author:		<Author: Arman Zakaryan>
-- Create date: <Create Date,,>
-- Description:	<Description: GetUsersByRoleId>
-- =============================================
CREATE PROCEDURE GetUsersByRoleId
	@RoleId bigint
AS
BEGIN
	SET NOCOUNT ON;

    SELECT [User].*
	FROM [User]
	INNER JOIN UserRole ON UserRole.UserId = [User].Id
	WHERE UserRole.RoleId = @RoleId
END