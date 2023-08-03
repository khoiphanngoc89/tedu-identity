using Microsoft.EntityFrameworkCore;
using Tedu.Identity.Common.Const;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    public static string GetConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("IdentitySqlConnection") ?? throw new ArgumentNullException("connectionStrings");
    }   
    
    public static void ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString();
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
        //.AddInMemoryIdentityResources(Config.IdentityResources)
        //.AddInMemoryApiScopes(Config.ApiScopes)
        //.AddInMemoryClients(Config.Clients)
        //.AddInMemoryApiResources(Config.ApiResources)
        //.AddTestUsers(TestUsers.Users)
        .AddConfigurationStore(opt =>
        {
            opt.ConfigureDbContext = c => c.UseSqlServer(connectionString,
                builder => builder.MigrationsAssembly(SystemConstants.AssemblyName));
        })
        .AddOperationalStore(opt =>
        {
            opt.ConfigureDbContext = c => c.UseSqlServer(connectionString,
                builder => builder.MigrationsAssembly(SystemConstants.AssemblyName));
        });
    }
}
