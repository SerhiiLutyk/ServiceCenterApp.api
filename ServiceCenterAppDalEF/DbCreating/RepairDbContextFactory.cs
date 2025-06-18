using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ServiceCenterAppDalEF
{
    public class RepairDbContextFactory : IDesignTimeDbContextFactory<RepairDbContext>
    {
        public RepairDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RepairDbContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Database=ServiceDb;Username=postgres;Password=root");

            return new RepairDbContext(optionsBuilder.Options);
        }
    }
}