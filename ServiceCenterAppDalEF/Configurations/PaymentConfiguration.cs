using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCenterAppDalEF.Entities;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.PaymentId);

        builder.Property(p => p.Amount);

        builder.Property(p => p.PaymentMethod)
               .HasMaxLength(50);

        builder.HasOne(p => p.Order)
               .WithMany(o => o.Payments)
               .HasForeignKey(p => p.OrderId);
    }
}
