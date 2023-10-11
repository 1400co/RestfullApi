using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Dtos;
using System;

namespace SocialMedia.Infrastructure.Data
{
    public class RolesConfiguration : IEntityTypeConfiguration<RolesDto>
    {
        public void Configure(EntityTypeBuilder<RolesDto> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("IdRol");
            builder.Property(e => e.RolName).HasColumnName("RolName").ValueGeneratedNever();
        }
    }
}
