﻿CREATE TABLE [dbo].[Clients] (
    [Id]                                    INT             IDENTITY (1, 1) NOT NULL,
    [Enabled]                               BIT             NOT NULL,
    [ClientId]                              NVARCHAR (200)  NOT NULL,
    [ProtocolType]                          NVARCHAR (200)  NOT NULL,
    [RequireClientSecret]                   BIT             NOT NULL,
    [ClientName]                            NVARCHAR (200)  NULL,
    [Description]                           NVARCHAR (1000) NULL,
    [ClientUri]                             NVARCHAR (2000) NULL,
    [LogoUri]                               NVARCHAR (2000) NULL,
    [RequireConsent]                        BIT             NOT NULL,
    [AllowRememberConsent]                  BIT             NOT NULL,
    [AlwaysIncludeUserClaimsInIdToken]      BIT             NOT NULL,
    [RequirePkce]                           BIT             NOT NULL,
    [AllowPlainTextPkce]                    BIT             NOT NULL,
    [RequireRequestObject]                  BIT             NOT NULL,
    [AllowAccessTokensViaBrowser]           BIT             NOT NULL,
    [RequireDPoP]                           BIT             NOT NULL,
    [DPoPValidationMode]                    INT             NOT NULL,
    [DPoPClockSkew]                         TIME (7)        NOT NULL,
    [FrontChannelLogoutUri]                 NVARCHAR (2000) NULL,
    [FrontChannelLogoutSessionRequired]     BIT             NOT NULL,
    [BackChannelLogoutUri]                  NVARCHAR (2000) NULL,
    [BackChannelLogoutSessionRequired]      BIT             NOT NULL,
    [AllowOfflineAccess]                    BIT             NOT NULL,
    [IdentityTokenLifetime]                 INT             NOT NULL,
    [AllowedIdentityTokenSigningAlgorithms] NVARCHAR (100)  NULL,
    [AccessTokenLifetime]                   INT             NOT NULL,
    [AuthorizationCodeLifetime]             INT             NOT NULL,
    [ConsentLifetime]                       INT             NULL,
    [AbsoluteRefreshTokenLifetime]          INT             NOT NULL,
    [SlidingRefreshTokenLifetime]           INT             NOT NULL,
    [RefreshTokenUsage]                     INT             NOT NULL,
    [UpdateAccessTokenClaimsOnRefresh]      BIT             NOT NULL,
    [RefreshTokenExpiration]                INT             NOT NULL,
    [AccessTokenType]                       INT             NOT NULL,
    [EnableLocalLogin]                      BIT             NOT NULL,
    [IncludeJwtId]                          BIT             NOT NULL,
    [AlwaysSendClientClaims]                BIT             NOT NULL,
    [ClientClaimsPrefix]                    NVARCHAR (200)  NULL,
    [PairWiseSubjectSalt]                   NVARCHAR (200)  NULL,
    [InitiateLoginUri]                      NVARCHAR (2000) NULL,
    [UserSsoLifetime]                       INT             NULL,
    [UserCodeType]                          NVARCHAR (100)  NULL,
    [DeviceCodeLifetime]                    INT             NOT NULL,
    [CibaLifetime]                          INT             NULL,
    [PollingInterval]                       INT             NULL,
    [CoordinateLifetimeWithUserSession]     BIT             NULL,
    [Created]                               DATETIME2 (7)   NOT NULL,
    [Updated]                               DATETIME2 (7)   NULL,
    [LastAccessed]                          DATETIME2 (7)   NULL,
    [NonEditable]                           BIT             NOT NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Clients_ClientId]
    ON [dbo].[Clients]([ClientId] ASC);

