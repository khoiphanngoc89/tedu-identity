CREATE TABLE [Identity].[User] (
    [Id]                   VARCHAR (50)       NOT NULL,
    [FirstName]            VARCHAR (50)       NOT NULL,
    [LastName]             VARCHAR (150)      NOT NULL,
    [Address]              NVARCHAR (MAX)     NOT NULL,
    [UserName]             VARCHAR (255)      NOT NULL,
    [NormalizedUserName]   VARCHAR (255)      NULL,
    [Email]                VARCHAR (255)      NOT NULL,
    [NormalizedEmail]      VARCHAR (255)      NULL,
    [EmailConfirmed]       BIT                NOT NULL,
    [PasswordHash]         NVARCHAR (MAX)     NULL,
    [SecurityStamp]        NVARCHAR (MAX)     NULL,
    [ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
    [PhoneNumber]          VARCHAR (20)       NULL,
    [PhoneNumberConfirmed] BIT                NOT NULL,
    [TwoFactorEnabled]     BIT                NOT NULL,
    [LockoutEnd]           DATETIMEOFFSET (7) NULL,
    [LockoutEnabled]       BIT                NOT NULL,
    [AccessFailedCount]    INT                NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_User_Email]
    ON [Identity].[User]([Email] ASC);

