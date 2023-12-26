using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Tedu.Identity.Infrastructure.Const;

namespace Tedu.Identity.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource
            {
                Name = SystemConstants.ConfigureOptions.Roles,
                DisplayName = "User role(s)",
                UserClaims = new List<string>
                {
                    SystemConstants.ConfigureOptions.Roles,
                }
            },
        };


    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new(SystemConstants.ConfigureOptions.Read, SystemConstants.ConfigureOptions.ReadDisplayName),
                new(SystemConstants.ConfigureOptions.Write, SystemConstants.ConfigureOptions.WriteDisplayName),
            };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
            {
                new (SystemConstants.ConfigureOptions.TeduApiName, SystemConstants.ConfigureOptions.DisplayName)
                {
                    Scopes = new List<string>()
                    {
                        SystemConstants.ConfigureOptions.Read,
                        SystemConstants.ConfigureOptions.Write
                    },
                    UserClaims = new List<string>()
                    {
                        SystemConstants.ConfigureOptions.Roles
                    }
                },
            };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new()
            {
                ClientName = SystemConstants.SwaggerClients.ClientName,
                ClientId = SystemConstants.SwaggerClients.ClientId,
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,
                AccessTokenLifetime = SystemConstants.SwaggerClients.TokenLifeTime,
                RedirectUris = new List<string>()
                {
                    "http://localhost:5001/swagger/oauth2-redirect.html",
                    "http://localhost:5002/swagger/oauth2-redirect.html"
                },
                PostLogoutRedirectUris = new List<string>()
                {
                    "http://localhost:5001/swagger/oauth2-redirect.html",
                    "http://localhost:5002/swagger/oauth2-redirect.html"
                },
                AllowedCorsOrigins = new List<string>()
                {
                    "http://localhost:5001",
                    "http://localhost:5002"
                },
                AllowedScopes = new List<string>()
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    SystemConstants.ConfigureOptions.TeduApiName,
                    SystemConstants.ConfigureOptions.Read,
                    SystemConstants.ConfigureOptions.Write
                }
            },
            new()
            {
                ClientName = SystemConstants.PostmanClients.ClientName,
                ClientId = SystemConstants.PostmanClients.ClientId,
                Enabled = true,
                ClientUri = null,
                RequireClientSecret = true,
                ClientSecrets = new[]
                {
                    new Secret(SystemConstants.PostmanClients.ClientSecret.Sha512())
                },
                AllowedGrantTypes = new[]
                {
                    GrantType.ClientCredentials,
                    GrantType.ResourceOwnerPassword
                },
                RequireConsent = false,
                AccessTokenLifetime = SystemConstants.SwaggerClients.TokenLifeTime,
                AllowOfflineAccess = true,
                RedirectUris = new List<string>()
                {
                    "https://wwww.getpostman.com/oauth2/callback"
                },
                AllowedScopes = new List<string>()
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    SystemConstants.ConfigureOptions.Roles,
                    SystemConstants.ConfigureOptions.Read,
                    SystemConstants.ConfigureOptions.Write
                }
            }
        };
}