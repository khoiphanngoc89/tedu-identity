using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Tedu.Identity.Common.Const;

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
                Name = Constants.Roles,
                UserClaims = new List<string>
                {
                    Constants.Roles,
                }
            },
        };

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy(Constants.CorsPolicy, builder => builder.AllowAnyOrigin()
                                                                                .AllowAnyMethod()
                                                                                .AllowAnyHeader());
            
        });
    }    

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new(Constants.TeduScopes.Read, Constants.TeduScopes.ReadDisplayName),
                new(Constants.TeduScopes.Write, Constants.TeduScopes.WriteDisplayName),
            };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
            {
                new (Constants.TeduApiResources.Name, Constants.TeduApiResources.DisplayName)
                {
                    Scopes = new List<string>()
                    {
                        Constants.TeduScopes.Read,
                        Constants.TeduScopes.Write
                    },
                    UserClaims = new List<string>()
                    {
                        Constants.Roles
                    }
                },
            };

    public static IEnumerable<Client> Clients =>
        new Client[] 
        {
            new()
            {
                ClientName = Constants.TeduClients.Swagger.ClientName,
                ClientId = Constants.TeduClients.Swagger.ClientId,
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,
                AccessTokenLifetime = Constants.TeduClients.Swagger.TokenLifeTime,
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
                    Constants.Roles,
                    Constants.TeduScopes.Read,
                    Constants.TeduScopes.Write
                }
            },
            new()
            {
                ClientName = Constants.TeduClients.Postman.ClientName,
                ClientId = Constants.TeduClients.Postman.ClientId,
                Enabled = true,
                ClientUri = null,
                RequireClientSecret = true,
                ClientSecrets = new[]
                {
                    new Secret("SuperStrongSecret".Sha512())
                },
                AllowedGrantTypes = new[]
                {
                    GrantType.ClientCredentials,
                    GrantType.ResourceOwnerPassword
                },
                RequireConsent = false,
                AccessTokenLifetime = Constants.TeduClients.Swagger.TokenLifeTime,
                AllowOfflineAccess = true,
                AllowedScopes = new List<string>()
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    Constants.Roles,
                    Constants.TeduScopes.Read,
                    Constants.TeduScopes.Write
                }
            }

        };
}