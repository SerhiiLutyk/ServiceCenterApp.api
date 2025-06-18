using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCenterAppDalEF.Entities;

public class AdditionalServiceConfiguration : IEntityTypeConfiguration<AdditionalService>
{
    public void Configure(EntityTypeBuilder<AdditionalService> builder)
    {
        builder.HasKey(s => s.ServiceId);

        builder.Property(s => s.Name)
               .HasMaxLength(100);

        builder.Property(s => s.Price);

        builder.HasMany(s => s.Orders)
               .WithOne(o => o.AdditionalService)
               .HasForeignKey(o => o.AdditionalServiceId);
    }
}
