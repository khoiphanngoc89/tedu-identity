CREATE TABLE [Identity].[Permissions] (
    [Id]       BIGINT       IDENTITY (1, 1) NOT NULL,
    [Function] VARCHAR (50) NOT NULL,
    [Command]  VARCHAR (50) NOT NULL,
    [RoleId]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC, [Command] ASC, [Function] ASC),
    CONSTRAINT [FK_Permissions_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Identity].[Role] ([Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Permissions_RoleId]
    ON [Identity].[Permissions]([RoleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Permissions_RoleId_Function_Command]
    ON [Identity].[Permissions]([RoleId] ASC, [Function] ASC, [Command] ASC);

