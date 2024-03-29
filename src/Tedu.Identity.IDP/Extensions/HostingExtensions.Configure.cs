﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Tedu.Identity.Infrastructure.Const;
using Tedu.Identity.Infrastructure.Settings;
using Tedu.Identity.Infrastructure.Entities;
using Tedu.Identity.Infrastructure.Persistence;
using Tedu.Identity.IDP.Services;
using Tedu.Identity.Infrastructure.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.AccessTokenValidation;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    public static string GetConnectionString(this IConfiguration configuration)
        => configuration.GetConnectionString("IdentitySqlConnection") ?? throw new ConnectionStringNotFoundException();

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

    public static IServiceCollection ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString();

        services.AddDbContext<TeduIdentityContext>(options => options.UseSqlServer(connectionString,
                        x => x.MigrationsAssembly(typeof(TeduIdentityContext).Assembly.FullName)));

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
                builder => builder.MigrationsAssembly(SystemConstants.ConfigureOptions.AssemblyName));
        })
        .AddOperationalStore(opt =>
        {
            opt.ConfigureDbContext = c => c.UseSqlServer(connectionString,
                builder => builder.MigrationsAssembly(SystemConstants.ConfigureOptions.AssemblyName));
        })
        .AddAspNetIdentity<User>()
        .AddProfileService<IdentityProfileService>();

        return services;
    }

    public static IServiceCollection ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
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
                .AddUserStore<TeduUserStore>()
                .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var identitySettings = configuration.GetSection(nameof(IdentityServerSettings))
            .Get<IdentityServerSettings>();

        if (identitySettings is null)
        {
            throw new NoNullAllowedException(nameof(identitySettings));
        }

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc(SystemConstants.ConfigureOptions.Version1, new()
            {
                Title = SystemConstants.ConfigureOptions.Title,
                Version = SystemConstants.ConfigureOptions.Version1,
                Contact = new()
                {
                    Name = SystemConstants.ConfigureOptions.TeduApiName,
                    Email = SystemConstants.ConfigureOptions.Email,
                    Url = new(identitySettings.ContactUrl)
                }
            });

            c.AddSecurityDefinition(IdentityServerAuthenticationDefaults.AuthenticationScheme, new()
            {
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.OAuth2,
                Flows = new()
                {
                    Implicit = new()
                    {
                        AuthorizationUrl = new($"{identitySettings.AuthorizeUrl}/connect/authorize"),
                        Scopes = new Dictionary<string, string>()
                        {
                            { SystemConstants.ConfigureOptions.Read, SystemConstants.ConfigureOptions.ReadDisplayName },
                            { SystemConstants.ConfigureOptions.Write, SystemConstants.ConfigureOptions.WriteDisplayName }
                        }
                    }
                }
            });

            c.AddSecurityRequirement(new()
            {
                {
                    new()
                    {
                        Reference = new()
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = IdentityServerAuthenticationDefaults.AuthenticationScheme,
                        },
                    },
                    new List<string>()
                    {
                        SystemConstants.ConfigureOptions.Read,
                        SystemConstants.ConfigureOptions.Write
                    }
                }
            });
        });

        return services;
    }

    public static IServiceCollection ConfigurationAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication()
            .AddLocalApi(IdentityServerAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.ExpectedScope = SystemConstants.ConfigureOptions.Read;
            });

        return services;
    }

    public static IServiceCollection ConfigurationAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(o =>
        {
            o.AddPolicy(IdentityServerAuthenticationDefaults.AuthenticationScheme, p =>
            {
                p.AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme);
                p.RequireAuthenticatedUser();
            });
        });

        return services;
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy(SystemConstants.ConfigureOptions.CorsPolicy, builder => builder.AllowAnyOrigin()
                                                                                .AllowAnyMethod()
                                                                                .AllowAnyHeader());

        });
    }
}
