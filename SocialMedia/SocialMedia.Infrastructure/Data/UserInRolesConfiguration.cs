using Microsoft.EntityFrameworkCore;
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


            /*builder
                .HasOne(d => d.Rol)
                .WithMany(p => p.UserInRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_USERINROLES_ROL_ROLMODULE");

            builder
                .HasOne(d => d.User)
                .WithMany(p => p.UserInRoles)
                .HasForeignKey(d => d.Users)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_USERINROLES_USER_ROLMODULE");*/
        }
    }
}
