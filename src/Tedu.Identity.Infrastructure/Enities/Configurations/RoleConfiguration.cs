using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tedu.Identity.Infrastructure.Const;

namespace Tedu.Identity.Infrastructure.Enities.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(new()
        {
            Name = SystemConstants.Seeders.Adminstrator,
            NormalizedName = SystemConstants.Seeders.Adminstrator.ToUpper(),
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        },
        new()
        {
            Name = SystemConstants.Seeders.Customer,
            NormalizedName = SystemConstants.Seeders.Customer.ToUpper(),
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        });
    }
}
