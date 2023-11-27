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
                UserClaims = new List<string>
                {
                    SystemConstants.ConfigureOptions.Roles,
                }
            },
        };

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy(SystemConstants.ConfigureOptions.CorsPolicy, builder => builder.AllowAnyOrigin()
                                                                                .AllowAnyMethod()
                                                                                .AllowAnyHeader());

        });
    }

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new(SystemConstants.TeduScopes.Read, SystemConstants.TeduScopes.ReadDisplayName),
                new(SystemConstants.TeduScopes.Write, SystemConstants.TeduScopes.WriteDisplayName),
            };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
            {
                new (SystemConstants.TeduApiResources.Name, SystemConstants.TeduApiResources.DisplayName)
                {
                    Scopes = new List<string>()
                    {
                        SystemConstants.TeduScopes.Read,
                        SystemConstants.TeduScopes.Write
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
                ClientName = SystemConstants.TeduClients.Swagger.ClientName,
                ClientId = SystemConstants.TeduClients.Swagger.ClientId,
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,
                AccessTokenLifetime = SystemConstants.TeduClients.Swagger.TokenLifeTime,
                RedirectUris = new List<string>()
                {
                    "http://localhost:5001/swagger/oauth2-redirect.html"
                },
                PostLogoutRedirectUris = new List<string>()
                {
                    "http://localhost:5001/swagger/oauth2-redirect.html"
                },
                AllowedCorsOrigins = new List<string>()
                {
                    "http://localhost:5001"
                },
                AllowedScopes = new List<string>()
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    SystemConstants.ConfigureOptions.Roles,
                    SystemConstants.TeduScopes.Read,
                    SystemConstants.TeduScopes.Write
                }
            },
            new()
            {
                ClientName = SystemConstants.TeduClients.Postman.ClientName,
                ClientId = SystemConstants.TeduClients.Postman.ClientId,
                Enabled = true,
                ClientUri = null,
                RequireClientSecret = true,
                ClientSecrets = new[]
                {
                    new Secret(SystemConstants.TeduClients.Postman.ClientSecret.Sha512())
                },
                AllowedGrantTypes = new[]
                {
                    GrantType.ClientCredentials,
                    GrantType.ResourceOwnerPassword
                },
                RequireConsent = false,
                AccessTokenLifetime = SystemConstants.TeduClients.Swagger.TokenLifeTime,
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
                    SystemConstants.TeduScopes.Read,
                    SystemConstants.TeduScopes.Write
                }
            }
        };
}