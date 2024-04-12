using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Infrastructure.Data.Configurations
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

            builder.HasOne(concepto => concepto.Roles)
             .WithMany(tipoAplicacion => tipoAplicacion.UserInRoles)
             .HasForeignKey(concepto => concepto.RoleId)
             .OnDelete(DeleteBehavior.NoAction)
             .IsRequired();

            builder.HasData(
                   new UserInRoles
                   {
                       Id = Guid.NewGuid(),
                       UserId = Guid.Parse("53AEECA4-A5B1-4751-ABCB-3207A01B97DC"),
                       RoleId = Guid.Parse("3A9A7CE2-9A5C-4AFF-A47A-C5FDFCD955AE"),
                       CreatedAt = DateTime.UtcNow,
                       Responsable = "System"
                   }
           );
        }
    }
}
