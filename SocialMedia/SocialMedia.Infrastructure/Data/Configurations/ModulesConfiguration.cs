using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Infrastructure.Data
{
    public class ModulesConfiguration : IEntityTypeConfiguration<Modules>
    {
        public void Configure(EntityTypeBuilder<Modules> builder)
        {
            builder.ToTable("Modulos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("IdModulo");
            builder.Property(e => e.ModuleName).HasColumnName("NombreModulo");

            builder.HasData(
                    new Modules
                    {
                        Id = new Guid("88b9cb17-dc3d-47e4-b60e-0bc75de3cae1"),
                        ModuleName = "Usuarios",
                        CreatedAt = DateTime.UtcNow,
                        Responsable = "System"
                    },
                    new Modules
                    {
                        Id = new Guid("d9e00690-bef3-483c-8275-49624fdeca2b"),
                        ModuleName = "Credenciales",
                        CreatedAt = DateTime.UtcNow,
                        Responsable = "System"
                    },
                    new Modules
                    {
                        Id = new Guid("642812ea-344e-4008-b4b6-4f74fba9b091"),
                        ModuleName = "Roles",
                        CreatedAt = DateTime.UtcNow,
                        Responsable = "System"
                    },
                    new Modules
                    {
                        Id = new Guid("1885be8d-aa27-4221-abdf-7affc845c63a"),
                        ModuleName = "Permisos",
                        CreatedAt = DateTime.UtcNow,
                        Responsable = "System"
                    },
                    new Modules
                    {
                        Id = new Guid("1885be8d-aa27-4221-abdf-7affc845c63b"),
                        ModuleName = "Home",
                        CreatedAt = DateTime.UtcNow,
                        Responsable = "System"
                    },
                    new Modules
                    {
                        Id = new Guid("24c51d74-d6f3-4409-a2b4-fccc8a4193b4"),
                        ModuleName = "Modules",
                        CreatedAt = DateTime.UtcNow,
                        Responsable = "System"
                    }
            );

        }
    }
}
