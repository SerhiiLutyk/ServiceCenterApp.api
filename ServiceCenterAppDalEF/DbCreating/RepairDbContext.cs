using Microsoft.EntityFrameworkCore;
using ServiceCenterAppDalEF.Configurations;
using ServiceCenterAppDalEF.Entities;

public class RepairDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<RepairType> RepairTypes { get; set; }
    public DbSet<AdditionalService> AdditionalServices { get; set; }
    public DbSet<Payment> Payments { get; set; }

    public RepairDbContext(DbContextOptions<RepairDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new RepairTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AdditionalServiceConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
    }
}
