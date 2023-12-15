using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using System;

namespace SocialMedia.Infrastructure.Data.Configurations
{
    public class SecurityConfiguration : IEntityTypeConfiguration<Security>
    {
        public void Configure(EntityTypeBuilder<Security> builder)
        {
            builder.ToTable("Seguridad");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("IdSeguridad");

            builder.Property(e => e.Role)
               .HasColumnName("Rol")
               .HasMaxLength(15)
               .IsRequired()
               .HasConversion(
                x => x.ToString(),
                x => (RoleType)Enum.Parse(typeof(RoleType), x)
                );

            builder.HasData(
                new Security()
                {
                    Id = Guid.NewGuid(),
                    UserId = new Guid("53aeeca4-a5b1-4751-abcb-3207a01b97dc"),
                    UserName = "admin",
                    Role = 0,
                    Password = "10000.mmlVX3xzYuLQromOzqELBQ==.JIwrJbVGsgYiTMjqWqcvulmXk8Fv6c7hxbl8mEqixTI=",

                }
                );
        }
    }
}
