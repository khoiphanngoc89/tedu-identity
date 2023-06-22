using Microsoft.EntityFrameworkCore;
using Tedu.Infrastructure.Configurations;
using Tedu.Infrastructure.Extensions;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    public static string GetAppConnectionString(this IServiceCollection services)
    {
        var dbSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
        if (dbSettings is null)
        {
            throw new ArgumentNullException(nameof(dbSettings));
        }
        return dbSettings.ConnectionString;
    }

    private static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

        if (databaseSettings is null)
        {
            throw new ArgumentNullException(nameof(databaseSettings));
        }
        services.AddSingleton(databaseSettings);

        return services;
    }

    private static IServiceCollection ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionSrting = services.GetAppConnectionString();
        services.AddIdentityServer(options =>
        {
            // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
            options.EmitStaticAudienceClaim = true;
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
        })
            .AddDeveloperSigningCredential()
            //.AddInMemoryIdentityResources(Config.IdentityResources)
            //.AddInMemoryApiScopes(Config.ApiScopes)
            //.AddInMemoryClients(Config.Clients)
            //.AddInMemoryApiResources(Config.ApiResources)
            //.AddTestUsers(TestUsers.Users)
            .AddConfigurationStore(opt =>
            {
                opt.ConfigureDbContext = c => c.UseSqlServer(connectionSrting, builder => builder.MigrationsAssembly("Tedu.Identity.IDP"));
            })
            .AddOperationalStore(opt =>
            {
                opt.ConfigureDbContext = c => c.UseSqlServer(connectionSrting, builder => builder.MigrationsAssembly("Tedu.Identity.IDP"));
            });
        return services;
    }

    private static IServiceCollection ConfigreCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
                                                                            .AllowAnyMethod()
                                                                            .AllowAnyHeader());
        });

        return services;
    }
}
