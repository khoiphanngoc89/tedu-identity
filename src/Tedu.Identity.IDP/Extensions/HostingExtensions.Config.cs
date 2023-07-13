using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tedu.Contracts.Entities;
using Tedu.Infrastructure.Configurations;
using Tedu.Infrastructure.Extensions;
using Tedu.Infrastructure.Persistence;

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

    private static IServiceCollection ConfigureIdentityServer(this IServiceCollection services)
    {
        var connectionString = services.GetAppConnectionString();
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
                .AddAspNetIdentity<User>()
                .AddConfigurationStore(opt =>
                {
                    opt.ConfigureDbContext = c => c.UseSqlServer(connectionString, builder => builder.MigrationsAssembly("Tedu.Identity.IDP"));
                })
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = c => c.UseSqlServer(connectionString, builder => builder.MigrationsAssembly("Tedu.Identity.IDP"));
                });
        return services;
    }

    private static void ConfigureIdentity(this IServiceCollection services)
    {
        var connectionString = services.GetAppConnectionString();
        services
            .AddDbContext<TeduIdentityContext>(options => options
                .UseSqlServer(connectionString))
            .AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            })
            .AddEntityFrameworkStores<TeduIdentityContext>()
            .AddDefaultTokenProviders();
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

    private static IServiceCollection ConfigureCookiePolicy(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            //.net core >3.0 should change value to SameSiteMode.Unspecified
            options.MinimumSameSitePolicy = (SameSiteMode)(-1);
            options.OnAppendCookie = cookieContext =>
                CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            options.OnDeleteCookie = cookieContext =>
                CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
        });

        return services;
    }

    static void CheckSameSite(HttpContext httpContext, CookieOptions options)
    {
        if (options.SameSite != SameSiteMode.None && options.SameSite != SameSiteMode.Unspecified) return;
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        if (DisallowsSameSiteNone(userAgent))
            //.net core >3.0 should change value to SameSiteMode.Unspecified
            options.SameSite = (SameSiteMode)(-1);
    }

    static bool DisallowsSameSiteNone(string userAgent)
    {
        // Cover all iOS based browsers here. This includes:
        //   - Safari on iOS 12 for iPhone, iPod Touch, iPad
        //   - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
        //   - Chrome on iOS 12 for iPhone, iPod Touch, iPad
        // All of which are broken by SameSite=None, because they use the
        // iOS networking stack.
        // Notes from Thinktecture:
        // Regarding https://caniuse.com/#search=samesite iOS versions lower
        // than 12 are not supporting SameSite at all. Starting with version 13
        // unknown values are NOT treated as strict anymore. Therefore we only
        // need to check version 12.
        if (userAgent.Contains("CPU iPhone OS 12")
            || userAgent.Contains("iPad; CPU OS 12"))
            return true;

        // Cover Mac OS X based browsers that use the Mac OS networking stack.
        // This includes:
        //   - Safari on Mac OS X.
        // This does not include:
        //   - Chrome on Mac OS X
        // because they do not use the Mac OS networking stack.
        // Notes from Thinktecture: 
        // Regarding https://caniuse.com/#search=samesite MacOS X versions lower
        // than 10.14 are not supporting SameSite at all. Starting with version
        // 10.15 unknown values are NOT treated as strict anymore. Therefore we
        // only need to check version 10.14.
        if (userAgent.Contains("Safari")
            && userAgent.Contains("Macintosh; Intel Mac OS X 10_14")
            && userAgent.Contains("Version/"))
            return true;

        // Cover Chrome 50-69, because some versions are broken by SameSite=None
        // and none in this range require it.
        // Note: this covers some pre-Chromium Edge versions,
        // but pre-Chromium Edge does not require SameSite=None.
        // Notes from Thinktecture:
        // We can not validate this assumption, but we trust Microsofts
        // evaluation. And overall not sending a SameSite value equals to the same
        // behavior as SameSite=None for these old versions anyways.
        if (userAgent.Contains("Chrome")) return true;

        return false;
    }
}
