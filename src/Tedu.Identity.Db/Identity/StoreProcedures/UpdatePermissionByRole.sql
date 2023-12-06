CREATE TYPE [dbo].[Permissions] AS TABLE (
	  [RoleId]   VARCHAR(50) NOT NULL
	, [Function] VARCHAR(50) NOT NULL
	, [Command]  VARCHAR(50) NOT NULL
);
GO

CREATE PROCEDURE [dbo].[UpdatePermissionByRole]
	  @roleId VARCHAR(50)
    , @permissions [dbo].[Permissions] READONLY
AS
BEGIN
	SET XACT_ABORT ON;
	BEGIN TRAN
	BEGIN TRY
	DELETE FROM [Identity].[Permissions]
	WHERE RoleId = @roleId

	INSERT INTO [Identity].[Permissions]([RoleId], [Function], [Command])
	SELECT [RoleId]
		 , [Function]
		 , [Command]
	FROM @permissions

	COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
		DECLARE @ErrorMessage VARCHAR(2000)
		SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
		RAISERROR(@ErrorMessage, 16, 1)
	END CATCH
END
