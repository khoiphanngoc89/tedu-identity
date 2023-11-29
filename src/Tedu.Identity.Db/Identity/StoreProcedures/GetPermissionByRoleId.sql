CREATE PROCEDURE [dbo].[GetPermissionByRoleId]
	@roleId int
AS
BEGIN
	SELECT *
	FROM [Identity].[Permissions]
	WHERE RoleId = @roleId
END
