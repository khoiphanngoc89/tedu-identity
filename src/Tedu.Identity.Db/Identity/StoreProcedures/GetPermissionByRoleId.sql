CREATE PROCEDURE [dbo].[GetPermissionByRoleId]
	@roleId VARCHAR(50)
AS
BEGIN
	SELECT *
	FROM [Identity].[Permissions]
	WHERE RoleId = @roleId
END
