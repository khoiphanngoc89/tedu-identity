using Microsoft.Extensions.Hosting;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    internal static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();
        builder.Host.AddAppConfiguration();

        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();
        builder.Services.ConfigureCookiePolicy();
        builder.Services.ConfigreCors();

        builder.Services.ConfigureIdentity();
        builder.Services.ConfigureIdentityServer();
        
        

        return builder.Build();
    }
}
