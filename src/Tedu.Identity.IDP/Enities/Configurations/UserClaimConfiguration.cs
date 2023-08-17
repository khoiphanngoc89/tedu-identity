using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tedu.Identity.Common.Const;

namespace Tedu.Identity.IDP.Enities.Configurations;

public class UserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        builder.ToTable(SystemConstants.Database.TableName.UserClaim, SystemConstants.Database.IdentityScheme)
           .HasKey(t => t.Id);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnType("varchar(50)");
    }
}
