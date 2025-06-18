using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCenterAppDalEF.Entities;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.OrderId);

        builder.Property(o => o.Status)
               .HasMaxLength(50);

        builder.Property(o => o.Description);

        builder.HasOne(o => o.Client)
               .WithMany(c => c.Orders)
               .HasForeignKey(o => o.ClientId);

        builder.HasOne(o => o.RepairType)
               .WithMany(r => r.Orders)
               .HasForeignKey(o => o.RepairTypeId);

        builder.HasOne(o => o.AdditionalService)
               .WithMany(s => s.Orders)
               .HasForeignKey(o => o.AdditionalServiceId)
               .IsRequired(false);
    }
}