CREATE PROCEDURE [dbo].[CreatePermission]
	@roleId VARCHAR(50)
  , @function VARCHAR(50)
  , @command VARCHAR(50)
  , @insertedId BIGINT OUTPUT
AS
BEGIN
	SET XACT_ABORT ON;
	BEGIN TRAN
	BEGIN TRY

	IF NOT EXISTS (SELECT TOP 1 1
				   FROM [Identity].[Permissions]
				   WHERE [RoleId] = @roleId
				     AND [Function] = @function
					 AND [Command] = @command)
	BEGIN
		INSERT INTO [Identity].[Permissions]([Function], [Command], [RoleId])
		VALUES (@function, @command, @roleId)

		SET @insertedId =  SCOPE_IDENTITY();
	END
	COMMIT

	END TRY
	BEGIN CATCH
		ROLLBACK
			DECLARE @ErrorMessage VARCHAR(2000)
			SELECT @ErrorMessage = 'Error: ' + ERROR_MESSAGE()
			RAISERROR(@ErrorMessage, 16, 1)
	END CATCH
END