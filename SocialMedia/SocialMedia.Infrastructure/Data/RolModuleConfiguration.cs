using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Infrastructure.Data
{
    public class RolModuleConfiguration : IEntityTypeConfiguration<RolModule>
    {
        public void Configure(EntityTypeBuilder<RolModule> builder)
        {
            builder.ToTable("RolModule");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("IdRolModule");

            builder.Property(e => e.Listed).HasColumnName("Listed").HasDefaultValue<bool>(false);
            builder.Property(e => e.Created).HasColumnName("Created").HasDefaultValue<bool>(false);
            builder.Property(e => e.Edited).HasColumnName("Edited").HasDefaultValue<bool>(false);
            builder.Property(e => e.Deleted).HasColumnName("Deleted").HasDefaultValue<bool>(false);
            builder.Property(e => e.Printed).HasColumnName("Printed").HasDefaultValue<bool>(false);

            builder.HasOne(d => d.Rol)
                .WithMany(p => p.RolModules)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_ROL_ROL_MODULE_CUSTOM");

        }
    }
}
