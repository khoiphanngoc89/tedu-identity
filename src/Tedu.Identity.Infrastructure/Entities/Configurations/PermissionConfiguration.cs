using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tedu.Identity.Infrastructure.Const;

namespace Tedu.Identity.Infrastructure.Entities.Configurations;

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
    }
}
