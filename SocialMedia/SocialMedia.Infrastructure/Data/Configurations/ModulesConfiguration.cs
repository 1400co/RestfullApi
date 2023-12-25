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
                    new Modules { Id = new Guid("88b9cb17-dc3d-47e4-b60e-0bc75de3cae1"), ModuleName = "Usuarios" },
                    new Modules { Id = new Guid("d9e00690-bef3-483c-8275-49624fdeca2b"), ModuleName = "Credenciales" },
                    new Modules { Id = new Guid("642812ea-344e-4008-b4b6-4f74fba9b091"), ModuleName = "Roles" },
                    new Modules { Id = new Guid("1885be8d-aa27-4221-abdf-7affc845c63a"), ModuleName = "Permisos" },
                    new Modules { Id = new Guid("1885be8d-aa27-4221-abdf-7affc845c63b"), ModuleName = "Home" },
                    new Modules { Id = new Guid("11d596ad-9247-4361-b0b1-28442eb6ac39"), ModuleName = "Maestra" },
                    new Modules { Id = new Guid("1aa226d1-c0a8-46b8-8e11-3eb8ff5a85a1"), ModuleName = "Detalle" },
                    new Modules { Id = new Guid("0213a14e-135a-409f-a33a-086841ea5e60"), ModuleName = "Actividades" },
                    new Modules { Id = new Guid("a6c6e63c-5c3b-4fb1-90ff-3fb2545e316e"), ModuleName = "Censo" },
                    new Modules { Id = new Guid("3ce81a2c-1c1b-4f32-8a1e-318e059be343"), ModuleName = "Espacios" },
                    new Modules { Id = new Guid("24c51d74-d6f3-4409-a2b4-fccc8a4193b4"), ModuleName = "Modules" },
                    new Modules { Id = new Guid("e10f9a9e-4d8b-469d-b673-01a93027380b"), ModuleName = "BasicTable" },
                    new Modules { Id = new Guid("46804cc0-e64d-4fb2-9d36-a17111059665"), ModuleName = "Siembra" },
                    new Modules { Id = new Guid("3a572694-1b31-4c75-9487-e8fa25fae402"), ModuleName = "Poda" },
                    new Modules { Id = new Guid("a567fed5-a980-4d28-9a53-96c0b535cab5"), ModuleName = "Tala" },
                    new Modules { Id = new Guid("200d3fc7-5563-4911-bb8e-e1c3f44de2b5"), ModuleName = "Solicitada" },
                    new Modules { Id = new Guid("2f8d5e30-d4f3-4243-a64b-d0f74b5dc966"), ModuleName = "Aprobada" },
                    new Modules { Id = new Guid("41d15ae0-350f-45b7-932c-5a1b65625db9"), ModuleName = "Ejecutada" },
                    new Modules { Id = new Guid("08f06231-7578-47cf-be04-1e59942f90f5"), ModuleName = "Rechazada" },
                    new Modules { Id = new Guid("9911c996-dcf7-4e1d-9bfe-620307dae777"), ModuleName = "Reportes" }


            );

        }
    }
}
