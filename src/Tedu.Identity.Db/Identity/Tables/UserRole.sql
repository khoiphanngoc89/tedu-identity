CREATE TABLE [Identity].[UserRole] (
    [UserId] VARCHAR (50) NOT NULL,
    [RoleId] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC)
);

