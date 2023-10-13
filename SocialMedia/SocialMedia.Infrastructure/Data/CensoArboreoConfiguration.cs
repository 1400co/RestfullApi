
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Data
{
    public class CensoArboreoConfiguration : IEntityTypeConfiguration<CensoArboreo>
    {

        public void Configure(EntityTypeBuilder<CensoArboreo> builder)
        {
            builder.ToTable("CensoArboreo");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("IdCensoArboreo");

        }
    }
    public class ActividadesSilviculturalesConfiguration : IEntityTypeConfiguration<ActividadesSilviculturales>
    {

        public void Configure(EntityTypeBuilder<ActividadesSilviculturales> builder)
        {
            builder.ToTable("ActividadesSilviculturales");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("IdActividadesSilviculturales");

        }
    }
    public class EspaciosPotencialesConfiguration : IEntityTypeConfiguration<EspaciosPotenciales>
    {

        public void Configure(EntityTypeBuilder<EspaciosPotenciales> builder)
        {
            builder.ToTable("EspaciosPotenciales");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("IdEspaciosPotenciales");

        }
    }
}
