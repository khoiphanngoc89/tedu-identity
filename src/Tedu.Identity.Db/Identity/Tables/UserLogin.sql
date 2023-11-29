CREATE TABLE [Identity].[UserLogin] (
    [UserId]              VARCHAR (50)   NOT NULL,
    [LoginProvider]       NVARCHAR (MAX) NOT NULL,
    [ProviderKey]         NVARCHAR (MAX) NOT NULL,
    [ProviderDisplayName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_UserLogin] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

