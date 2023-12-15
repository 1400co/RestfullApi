using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
             .HasColumnName("IdUsuario").ValueGeneratedNever();

            builder.HasData(
                new User()
                {
                    Id = new Guid("53aeeca4-a5b1-4751-abcb-3207a01b97dc"),
                    FullName = "Oscar",
                    IsActive = true,
                    Email = "oruedar@yopmail.com",
                }
                );
        }
    }
}
