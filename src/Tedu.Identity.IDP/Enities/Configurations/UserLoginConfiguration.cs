﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tedu.Identity.Common.Const;

namespace Tedu.Identity.IDP.Enities.Configurations;

public class UserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.ToTable(SystemConstants.Database.TableName.UserLogin, SystemConstants.Database.IdentityScheme)
           .HasKey(t => t.UserId);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnType("varchar(50)");
    }
}
