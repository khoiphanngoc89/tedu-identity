CREATE TABLE [Identity].[UserToken] (
    [UserId]        VARCHAR (50)   NOT NULL,
    [LoginProvider] NVARCHAR (MAX) NOT NULL,
    [Name]          NVARCHAR (MAX) NOT NULL,
    [Value]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_UserToken] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

