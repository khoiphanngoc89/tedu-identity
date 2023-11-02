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
        
        return builder.Build();
    }
}
