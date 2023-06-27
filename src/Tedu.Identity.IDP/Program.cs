using Serilog;
using Tedu.Identity.IDP.Extensions;
using Tedu.Identity.IDP.Persistence;

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
Log.Information($"Starting {builder.Environment.ApplicationName} up");

try
{
    var app = builder
     .ConfigureServices()
     .ConfigurePipeline();
    app.MigrateDatabase();
    SeedUser.EnsureSeedData(builder.Services.GetAppConnectionString());
    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    const string StopTheHostException = nameof(StopTheHostException);
    if (type.Equals(StopTheHostException)) throw;
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}