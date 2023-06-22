using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace Tedu.Identity.IDP.Persistence;

public static class IdentitySeed
{
    public static IHost MigrationDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        scope.ServiceProvider
             .GetRequiredService<PersistedGrantDbContext>()
             .Database
             .Migrate();

        using var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
        try
        {
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach(var client in Config.Clients)
                {

                }
            }
        }
        catch (Exception ex) { }
        return host;
    }
}
