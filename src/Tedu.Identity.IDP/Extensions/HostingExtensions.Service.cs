using Microsoft.Extensions.Hosting;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    internal static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.AddAppConfiguration();

        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();
        builder.Services.ConfigureIdentity();
        builder.Services.ConfigureIdentityServer();
        
        builder.Services.ConfigreCors();

        return builder.Build();
    }
}
