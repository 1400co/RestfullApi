using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Data.Configurations
{
    public class CervezasConfiguration : IEntityTypeConfiguration<Cervezas>
    {
        public void Configure(EntityTypeBuilder<Cervezas> builder)
        {
            builder.ToTable("Cerveza");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("IdCerveza")
                .ValueGeneratedNever();

            builder.Property(e => e.Nombre)
                .HasColumnName("Nombre")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.GradosAlcohol)
                .HasColumnName("GradosAlcohol")
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(e => e.Precio)
                .HasColumnName("Precio")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(e => e.Responsable)
                .HasColumnName("Responsable")
                .HasMaxLength(100);
        }
    }
}
