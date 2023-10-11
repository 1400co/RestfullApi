using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Dtos;
using System;

namespace SocialMedia.Infrastructure.Data
{
    public class UserInRolesConfiguration : IEntityTypeConfiguration<UserInRolesDto>
    {
        public void Configure(EntityTypeBuilder<UserInRolesDto> builder)
        {
            builder.ToTable("UserInRoles");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("IdUserInRoles");

            builder.Property(e => e.UserId).HasColumnName("IdUser").ValueGeneratedNever();
            builder.Property(e => e.RoleId).HasColumnName("IdRol").ValueGeneratedNever();
        }
    }
}
