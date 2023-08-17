using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tedu.Identity.Common.Const;

namespace Tedu.Identity.IDP.Enities.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.ToTable(SystemConstants.Database.TableName.Role, SystemConstants.Database.IdentityScheme)
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .IsRequired()
               .HasColumnType("varchar(50)");

        builder.Property(x => x.Name)
               .IsRequired()
               .IsUnicode()
               .HasColumnType("nvarchar(150)")
               .HasMaxLength(150);

        builder.HasData(new()
        {
            Name = "Adminstrator",
            NormalizedName = "ADMINSTRATOR",
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        },
        new()
        {
            Name = "Customer",
            NormalizedName = "CUSTOMER",
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        });
    }
}
