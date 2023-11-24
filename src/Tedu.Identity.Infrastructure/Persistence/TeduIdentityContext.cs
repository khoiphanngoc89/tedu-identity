using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tedu.Identity.Infrastructure.Enities;

namespace Tedu.Identity.Infrastructure.Persistence;

public class TeduIdentityContext : IdentityDbContext<User>
{
    public DbSet<Permission> Permissions { get; set; }
    public TeduIdentityContext(DbContextOptions<TeduIdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.ApplyConfiguration(new RoleConfiguration());
        //apply configurations from same assembly
        builder.ApplyConfigurationsFromAssembly(typeof(TeduIdentityContext).Assembly);
        builder.ApplyIdentityConfiguration();
    }
}
