CREATE TABLE [Identity].[Permissions] (
    [Id]       BIGINT       IDENTITY (1, 1) NOT NULL,
    [Function] VARCHAR (50) NOT NULL,
    [Command]  VARCHAR (50) NOT NULL,
    [RoleId]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC, [Command] ASC, [Function] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Permissions_RoleId_Function_Command]
    ON [Identity].[Permissions]([RoleId] ASC, [Function] ASC, [Command] ASC);

