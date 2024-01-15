using Serilog;
using Tedu.Identity.IDP.Extensions;
using Tedu.Identity.IDP.Persistence;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");
var builder = WebApplication.CreateBuilder(args);
try
{
    builder.Host.AddAppConfigurations();

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    app.MigrateDatabase();
    if (app.Environment.IsDevelopment())
    {
        SeedUser.EnsureSeedUser(builder.Configuration.GetConnectionString());
    }

    app.Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (string.Equals(type, "StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    if (string.Equals(type, "HostAbortedException", StringComparison.Ordinal))
    {
        return;
    }

    Log.Fatal(ex, $"Unhandles exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}