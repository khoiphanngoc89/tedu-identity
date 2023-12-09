using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tedu.Identity.Infrastructure.Const;

namespace Tedu.Identity.Infrastructure.Entities.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(new()
        {
            Name = SystemConstants.ConfigureOptions.Administrator,
            NormalizedName = SystemConstants.ConfigureOptions.Administrator.ToUpper(),
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        },
        new()
        {
            Name = SystemConstants.ConfigureOptions.Customer,
            NormalizedName = SystemConstants.ConfigureOptions.Customer.ToUpper(),
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        });
    }
}
