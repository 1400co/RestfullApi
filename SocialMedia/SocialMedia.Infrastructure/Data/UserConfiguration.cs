using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Infrastructure.Data
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
                new User() { Id = Guid.NewGuid(), FullName = "Oscar", IsActive = true, Email = "oruedar@yopmail.com" }
                );
        }
    }
}
