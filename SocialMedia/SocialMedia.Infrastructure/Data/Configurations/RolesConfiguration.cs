using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Infrastructure.Data.Configurations
{
    public class RolesConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("IdRol");
            builder.Property(e => e.RolName).HasColumnName("RolName").ValueGeneratedNever();

            builder.HasData(
              new Roles()
              {
                  Id = new Guid("3A9A7CE2-9A5C-4AFF-A47A-C5FDFCD955AE"),
                  RolName = "Super Administrator",
                  CreatedAt = DateTime.UtcNow,
                  Responsable = "System",
              },
               new Roles()
               {
                   Id = new Guid("7C2E1E9B-410B-4A6B-B9AE-8B078422EB2D"),
                   RolName = "Administrator",
                   CreatedAt = DateTime.UtcNow,
                   Responsable = "System",
               }
              );
        }
    }
}
