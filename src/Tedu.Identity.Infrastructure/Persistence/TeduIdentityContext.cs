using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection.Emit;
using Tedu.Identity.Infrastructure.Enities;

namespace Tedu.Identity.Infrastructure.Persistence;

public class TeduIdentityContext : IdentityDbContext<User>
{
    public DbSet<Permission> Permissions { get; set; }
    public IDbConnection Connection => Database.GetDbConnection();
    public TeduIdentityContext(DbContextOptions<TeduIdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //apply configurations from same assembly
        builder.ApplyConfigurationsFromAssembly(typeof(TeduIdentityContext).Assembly);
        builder.ApplyIdentityConfiguration();
    }
}
