using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Core.Entities;

namespace Template.Infrastructure.Data.Configurations
{
    public class OtpConfiguration : IEntityTypeConfiguration<Otp>
    {
        public void Configure(EntityTypeBuilder<Otp> builder)
        {
            builder.ToTable("Otp");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
               .HasColumnName("IdOtp").ValueGeneratedNever();
        }
    }
}
