using Serilog;
using Tedu.Identity.IDP.Utilities;

namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    internal static ConfigureHostBuilder AddAppConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((cxt, conf) =>
        {
            var env = cxt.HostingEnvironment;
            conf.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }).UseSerilog(Serilogger.Configure);

        return host;
    }
}
