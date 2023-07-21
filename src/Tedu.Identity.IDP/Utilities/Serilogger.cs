using Serilog;

namespace Tedu.Identity.IDP.Utilities;

public static class Serilogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure => (context, configuration) =>
    {
        var appName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
        var envName = context.HostingEnvironment.EnvironmentName ?? "Development";

        var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
        var username = context.Configuration.GetValue<string>("ElasticConfiguration:Username");
        var password = context.Configuration.GetValue<string>("ElasticConfiguration:Password");

        if (elasticUri is null || username is null || password is null)
        {
            throw new InvalidOperationException("Elastic is not configured");
        }

        configuration
            .WriteTo.Debug()
            .WriteTo.Console(outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
            .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                IndexFormat = $"tedulogs-{appName}-{envName}-{DateTime.UtcNow:yyyy-MM}",
                AutoRegisterTemplate = true,
                NumberOfReplicas = 1,
                NumberOfShards = 2,
                ModifyConnectionSettings = x => x.BasicAuthentication(username, password)
            })
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Environment", envName)
            .Enrich.WithProperty("Application", appName)
            .ReadFrom.Configuration(context.Configuration);
    };
}
