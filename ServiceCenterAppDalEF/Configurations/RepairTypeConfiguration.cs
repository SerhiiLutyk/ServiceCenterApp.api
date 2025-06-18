using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCenterAppDalEF.Entities;

public class RepairTypeConfiguration : IEntityTypeConfiguration<RepairType>
{
    public void Configure(EntityTypeBuilder<RepairType> builder)
    {
        builder.HasKey(r => r.RepairTypeId);

        builder.Property(r => r.Name)
               .HasMaxLength(100);

        builder.Property(r => r.Price);

        builder.HasMany(r => r.Orders)
               .WithOne(o => o.RepairType)
               .HasForeignKey(o => o.RepairTypeId);
    }
}
