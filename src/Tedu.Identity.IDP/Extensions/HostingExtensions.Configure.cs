using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Tedu.Identity.Common.Const;
using Tedu.Identity.IDP.Common.Settings;
using Tedu.Identity.IDP.Enities;
using Tedu.Identity.IDP.Persistence;
using Tedu.Identity.IDP.Services;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    public static string GetConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("IdentitySqlConnection") ?? throw new ArgumentNullException("connectionStrings");
    }
    
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection(nameof(SmtpEmailSettings))
            .Get<SmtpEmailSettings>();

        if (emailSettings is null)
        {
            throw new NoNullAllowedException(nameof(emailSettings));
        }

        services.AddSingleton(emailSettings);

        return services;
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
        .AddConfigurationStore(opt =>
        {
            opt.ConfigureDbContext = c => c.UseSqlServer(connectionString,
                builder => builder.MigrationsAssembly(SystemConstants.AssemblyName));
        })
        .AddOperationalStore(opt =>
        {
            opt.ConfigureDbContext = c => c.UseSqlServer(connectionString,
                builder => builder.MigrationsAssembly(SystemConstants.AssemblyName));
        })
        .AddAspNetIdentity<User>()
        .AddProfileService<IdentityProfileService>();
    }

    public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString();

        services.AddDbContext<TeduIdentityContext>(options =>
                    options.UseSqlServer(connectionString))
                .AddIdentity<User, IdentityRole>(options => 
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                })
                .AddEntityFrameworkStores<TeduIdentityContext>()
                .AddDefaultTokenProviders();
    }    
}
