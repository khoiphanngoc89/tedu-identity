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
                Name = Constants.Role,
                UserClaims = new List<string>
                {
                    Constants.Role,
                }
            },
        };

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
                        Constants.Role
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
                AccessTokenLifetime = Constants.TeduClients.Swagger.TokenLifeTime
            }

        };
}