using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCenterAppDalEF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterAppDalEF.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(c => c.ClientId);

            builder.Property(c => c.FirstName)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(c => c.Phone)
                   .HasMaxLength(20)
                   .IsRequired(false);

            builder.HasMany(c => c.Orders)
                   .WithOne(o => o.Client)
                   .HasForeignKey(o => o.ClientId);
        }
    }

}
