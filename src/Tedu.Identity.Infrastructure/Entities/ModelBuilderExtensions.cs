using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tedu.Identity.Infrastructure.Const;


namespace Tedu.Identity.Infrastructure.Entities;

public static class ModelBuilderExtensions
{
    public static void ApplyIdentityConfiguration(this ModelBuilder builder)
    {
        ConfigureRole(builder.Entity<IdentityRole>());
        ConfigureUser(builder.Entity<User>());
        ConfigureRoleClaim(builder.Entity<IdentityRoleClaim<string>>());
        ConfigureUserRole(builder.Entity<IdentityUserRole<string>>());
        ConfigureUserClaim(builder.Entity<IdentityUserClaim<string>>());
        ConfigureUserLogin(builder.Entity<IdentityUserLogin<string>>());
        ConfigureUserToken(builder.Entity<IdentityUserToken<string>>());
    }

    private static void ConfigureUserToken(EntityTypeBuilder<IdentityUserToken<string>> entity)
    {
        entity.ToTable(SystemConstants.Database.TableNames.UserToken, SystemConstants.Database.Schemes.IdentityScheme)
            .HasKey(x => new { x.UserId });

        entity
            .Property(x => x.UserId)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50);
    }

    private static void ConfigureUserLogin(EntityTypeBuilder<IdentityUserLogin<string>> entity)
    {
        entity.ToTable(SystemConstants.Database.TableNames.UserLogin, SystemConstants.Database.Schemes.IdentityScheme)
            .HasKey(x => x.UserId);

        entity
            .Property(x => x.UserId)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50);
    }

    private static void ConfigureUserClaim(EntityTypeBuilder<IdentityUserClaim<string>> entity)
    {
        entity.ToTable(SystemConstants.Database.TableNames.UserClaim, SystemConstants.Database.Schemes.IdentityScheme)
            .HasKey(x => x.Id);

        entity
            .Property(x => x.UserId)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50);
    }

    private static void ConfigureUserRole(EntityTypeBuilder<IdentityUserRole<string>> entity)
    {
        entity.ToTable(SystemConstants.Database.TableNames.UserRole, SystemConstants.Database.Schemes.IdentityScheme)
            .HasKey(x => new { x.UserId, x.RoleId });

        entity
            .Property(x => x.UserId)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50);
        entity
            .Property(x => x.RoleId)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50);
    }

    private static void ConfigureRole(EntityTypeBuilder<IdentityRole> entity)
    {
        entity.ToTable(SystemConstants.Database.TableNames.Role, SystemConstants.Database.Schemes.IdentityScheme)
            .HasKey(x => x.Id);

        entity
            .Property(x => x.Id)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50);

        entity
            .Property(x => x.Name)
            .IsRequired()
            .IsUnicode()
            .HasColumnType(SystemConstants.Database.DataTypes.Nvarchar150)
            .HasMaxLength(150);
    }

    private static void ConfigureUser(EntityTypeBuilder<User> entity)
    {
        entity.ToTable(SystemConstants.Database.TableNames.User, SystemConstants.Database.Schemes.IdentityScheme)
            .HasKey(x => x.Id);

        entity
            .Property(x => x.Id)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50);

        entity.HasIndex(x => x.Email);
        entity.Property(x => x.Email)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar255)
            .HasMaxLength(255)
            .ValueGeneratedNever();

        entity.Property(x => x.NormalizedEmail)
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar255)
            .HasMaxLength(255);

        entity.Property(x => x.UserName)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar255)
            .HasMaxLength(255);

        entity.Property(x => x.NormalizedUserName)
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar255)
            .HasMaxLength(255)
            ;

        entity.Property(x => x.PhoneNumber)
            .IsUnicode(false)
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar20)
            .HasMaxLength(20);

        entity.Property(x => x.FirstName)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50)
            .HasMaxLength(50);

        entity.Property(x => x.LastName)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar150)
            .HasMaxLength(150);
    }

    private static void ConfigureRoleClaim(EntityTypeBuilder<IdentityRoleClaim<string>> entity)
    {
        entity.ToTable(SystemConstants.Database.TableNames.RoleClaim, SystemConstants.Database.Schemes.IdentityScheme)
            .HasKey(x => x.Id);

        entity
            .Property(x => x.Id)
            .IsRequired()
            .HasColumnType(SystemConstants.Database.DataTypes.Varchar50);
    }
}