CREATE PROCEDURE [dbo].[DeletePermission]
	@roleId   varchar(50)
  , @function varchar(50)
  , @command  varchar(50)
AS
BEGIN
	DELETE FROM [Identity].[Permissions]
	WHERE [RoleId]   = @roleId
	  AND [Function] = @function
	  AND [Command]  = @command
END