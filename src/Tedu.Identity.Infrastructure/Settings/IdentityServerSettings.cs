namespace Tedu.Identity.Infrastructure.Settings;

public sealed class IdentityServerSettings
{
    public string? BaseUrl { get; set; }
    public string AuthorizeUrl { get; set; } = string.Empty;
    public string ContactUrl { get; set; } = string.Empty;
}
