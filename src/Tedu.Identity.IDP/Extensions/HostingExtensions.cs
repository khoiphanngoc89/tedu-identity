using Serilog;
using Serilog.Sinks.Elasticsearch;
using Tedu.Infrastructure.Configurations;
using Tedu.Infrastructure.Extensions;


namespace Tedu.Identity.IDP.Extensions;

internal static partial class HostingExtensions
{
    private static IHostBuilder AddAppConfiguration(this IHostBuilder host)
    {
        host.ConfigureAppConfiguration((cxt, conf) =>
        {
            var env = cxt.HostingEnvironment;
            conf.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }).ConfigureSerilog();

        return host;
    }

    private static IHostBuilder ConfigureSerilog(this IHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
        {
            var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
            var username = context.Configuration.GetValue<string>("ElasticConfiguration:Username");
            var password = context.Configuration.GetValue<string>("ElasticConfiguration:Password");
            var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");

            if (string.IsNullOrEmpty(elasticUri))
            {
                throw new Exception("ElasticConfiguration Uri is not configured.");
            }

            configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console().ReadFrom.Configuration(context.Configuration)
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticUri))
                    {
                        IndexFormat =
                            $"{applicationName}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                        AutoRegisterTemplate = true,
                        NumberOfShards = 2,
                        NumberOfReplicas = 1,
                        ModifyConnectionSettings = x => x.BasicAuthentication(username, password)
                    })
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", applicationName)
                .ReadFrom.Configuration(context.Configuration);
        });

        return host;
    }

    public static string GetConnectionString(this IServiceCollection services)
    {
        var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
        if (databaseSettings is null)
        {
            throw new ArgumentNullException(nameof(databaseSettings));
        }
        return databaseSettings.ConnectionString;
    }
}
