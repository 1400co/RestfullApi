
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
}
