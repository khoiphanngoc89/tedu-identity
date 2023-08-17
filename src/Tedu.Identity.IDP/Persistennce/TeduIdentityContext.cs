using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tedu.Identity.IDP.Enities;

namespace Tedu.Identity.IDP.Persistennce;

public class TeduIdentityContext : IdentityDbContext<User>
{
    public TeduIdentityContext(DbContextOptions<TeduIdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.ApplyConfiguration(new RoleConfiguration());
        //apply configurations from same assembly
        builder.ApplyConfigurationsFromAssembly(typeof(TeduIdentityContext).Assembly);
        base.OnModelCreating(builder);
    }
}
