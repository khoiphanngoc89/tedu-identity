namespace Tedu.Infrastructure.Configurations;

public sealed class DatabaseSettings
{
    public string? DbProvider { get; set; }
    public string? ConnectionString { get; set; }
}
