﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Infrastructure.Data
{
    public class UserInRolesConfiguration : IEntityTypeConfiguration<UserInRoles>
    {
        public void Configure(EntityTypeBuilder<UserInRoles> builder)
        {
            builder.ToTable("UserInRoles");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("IdUserInRoles");

            builder.Property(e => e.UserId).HasColumnName("IdUser").ValueGeneratedNever();
            builder.Property(e => e.RoleId).HasColumnName("IdRol").ValueGeneratedNever();
        }
    }
}
