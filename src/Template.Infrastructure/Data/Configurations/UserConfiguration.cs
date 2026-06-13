using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Core.Entities;
using Template.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Template.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
             .HasColumnName("IdUsuario").ValueGeneratedNever();

            builder.Property(e => e.Roles)
                .HasColumnName("Roles")
                .HasConversion(
                    v => string.Join(',', v),
                    v => string.IsNullOrEmpty(v)
                        ? new List<RoleType>()
                        : v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => Enum.Parse<RoleType>(s))
                            .ToList()
                )
                .IsRequired();

            builder.HasData(
                new User()
                {
                    Id = new Guid("53aeeca4-a5b1-4751-abcb-3207a01b97dc"),
                    FullName = "Oscar",
                    IsActive = true,
                    Email = "oruedar@yopmail.com",
                    Roles = new List<RoleType> { RoleType.Administrator },
                    CreatedAt = DateTime.UtcNow,
                    Responsable = "System"
                }
                );
        }
    }
}
