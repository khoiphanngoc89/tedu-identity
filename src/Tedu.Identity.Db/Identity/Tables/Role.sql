CREATE TABLE [Identity].[Role] (
    [Id]               VARCHAR (50)   NOT NULL,
    [Name]             NVARCHAR (150) NOT NULL,
    [NormalizedName]   NVARCHAR (MAX) NULL,
    [ConcurrencyStamp] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id] ASC)
);

