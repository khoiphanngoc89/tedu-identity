CREATE TABLE [Identity].[RoleClaim] (
    [Id]         VARCHAR (50)   NOT NULL,
    [RoleId]     NVARCHAR (MAX) NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_RoleClaim] PRIMARY KEY CLUSTERED ([Id] ASC)
);

