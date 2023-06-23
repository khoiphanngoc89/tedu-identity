using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Tedu.Contracts.Entities;

namespace Tedu.Infrastructure.Persistence;

public class TeduIdentityContext : IdentityDbContext<User>
{
    public IDbConnection Connection => Database.GetDbConnection();

    public TeduIdentityContext(DbContextOptions<TeduIdentityContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(TeduIdentityContext).Assembly);
    }
}
