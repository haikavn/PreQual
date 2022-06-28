CREATE PROCEDURE [dbo].[GetUsersWithRolesByRoleId]
	@RoleId bigint
AS
BEGIN
	SET NOCOUNT ON;

   SELECT [User].Id, [User].LoginDate, [User].Deleted, [Role].[Name] AS RoleName, 
	       [Profile].FirstName, [Profile].LastName, [User].Active, [User].UserType
	FROM [User]
	INNER JOIN UserRole ON UserRole.UserId = [User].Id
	INNER JOIN [Profile] ON[Profile].UserId = [User].Id
	LEFT JOIN [Role] On [Role].Id = UserRole.RoleId 
	WHERE UserRole.RoleId = @RoleId
END