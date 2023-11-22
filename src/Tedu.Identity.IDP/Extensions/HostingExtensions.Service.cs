using Tedu.Identity.IDP.Enities.Domains;
using Tedu.Identity.IDP.Repositories;
using Tedu.Identity.IDP.Services;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.AddConfigurationSettings(builder.Configuration);
        builder.Services.ConfigureCookiePolicy();
        builder.Services.ConfigureCors();

        builder.Services.AddScoped<IEmailSender, SmtpMailService>();

        builder.Services.ConfigureIdentity(builder.Configuration);
        builder.Services.ConfigureIdentityServer(builder.Configuration);

        builder.Services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
        builder.Services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
        builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
        

        return builder.Build();
    }
}
