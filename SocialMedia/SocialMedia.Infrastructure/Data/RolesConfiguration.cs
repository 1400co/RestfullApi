using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Infrastructure.Data
{
    public class RolesConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("IdRol");
            builder.Property(e => e.RolName).HasColumnName("RolName").ValueGeneratedNever();
        }
    }
}
