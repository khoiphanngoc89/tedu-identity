using Microsoft.Extensions.Hosting;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    internal static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.AddAppConfiguration();

        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();
        builder.Services.ConfigureIdentityServer(builder.Configuration);
        builder.Services.ConfigreCors();

        return builder.Build();
    }
}
