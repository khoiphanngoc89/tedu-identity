using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tedu.Identity.Common.Const;

namespace Tedu.Identity.IDP.Enities.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(SystemConstants.Database.TableName.User, SystemConstants.Database.IdentityScheme)
            .HasKey(t => t.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnType("varchar(255)")
            .HasMaxLength(255)
            .ValueGeneratedNever();

        builder.HasIndex(x => x.Email);

        builder.Property<string>(x => x.UserName)
            .IsRequired()
            .HasColumnType("varchar(255)")
            .HasMaxLength(255);

        builder.Property(x => x.NormalizedUserName)
            .HasColumnType("varchar(255)")
            .HasMaxLength(255);

        builder.Property (x => x.PhoneNumber)
            .IsUnicode(false)
            .HasColumnType("varchar(20)")
            .HasMaxLength(20);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasColumnType("varchar(150)")
            .HasMaxLength(150);
    }
}
