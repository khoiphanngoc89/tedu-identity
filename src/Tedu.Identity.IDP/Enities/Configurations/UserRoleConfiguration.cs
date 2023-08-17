using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tedu.Identity.Common.Const;

namespace Tedu.Identity.IDP.Enities.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.ToTable(SystemConstants.Database.TableName.UserRole, SystemConstants.Database.IdentityScheme)
            .HasKey(x => new { x.UserId, x.RoleId });

        builder.Property(x => x.UserId)
           .IsRequired()
           .HasColumnType("varchar(50)");

        builder.Property(x => x.RoleId)
           .IsRequired()
           .HasColumnType("varchar(50)");
    }
}
