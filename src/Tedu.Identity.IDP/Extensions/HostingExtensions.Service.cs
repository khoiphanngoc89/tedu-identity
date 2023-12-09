using Tedu.Identity.Infrastructure.Domains;
using Tedu.Identity.Infrastructure.Repositories;
using Tedu.Identity.IDP.Services;
using Tedu.Identity.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using Tedu.Identity.Presentation;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();
        builder.Services.AddConfigurationSettings(builder.Configuration);

        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.ConfigureCookiePolicy();
        builder.Services.ConfigureCors();

        builder.Services.AddScoped<IEmailSender, SmtpMailService>();

        builder.Services.ConfigureIdentity(builder.Configuration);
        builder.Services.ConfigureIdentityServer(builder.Configuration);

        builder.Services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
        builder.Services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
        builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

        builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
            config.Filters.Add(new ProducesAttribute("application/json", "text/plain", "text/json"));
        }).AddApplicationPart(typeof(IAssemblyReference).Assembly);

        builder.Services.ConfigurationAuthentication();
        builder.Services.ConfigurationAuthorization();

        builder.Services.ConfigureSwagger(builder.Configuration);
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        return builder.Build();
    }
}
