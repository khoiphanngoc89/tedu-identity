using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tedu.Identity.Common.Const;

namespace Tedu.Identity.IDP.Enities.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(SystemConstants.Database.TableNames.Permissions, SystemConstants.Database.Schemes.IdentityScheme)
            .HasKey(x => new { x.Id, x.Command, x.Function });

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(x => new { x.RoleId, x.Function, x.Command });

        builder.Property(x => x.Command)
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50)
            .HasMaxLength(50);

        builder.Property(x => x.Function)
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50)
            .HasMaxLength(50);

        builder.Property(x => x.RoleId)
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(x => x.Role)
            .WithOne()
            .HasForeignKey<Permission>(x => x.RoleId);
    }
}
