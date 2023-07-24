using Serilog;
using Tedu.Identity.IDP.Utilities;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    public static void ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityServer(options =>
        {
            // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
            options.EmitStaticAudienceClaim = true;
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
        })
        // not recommend for productions
        .AddDeveloperSigningCredential()
        .AddInMemoryIdentityResources(Config.IdentityResources)
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryClients(Config.Clients)
        .AddInMemoryApiResources(Config.ApiResources)
        .AddTestUsers(TestUsers.Users);
    }
}
