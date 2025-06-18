using Bogus;
using ServiceCenterAppDalEF.Entities;

namespace RepairServiceDAL.DbCreating.DataGeneration
{
    public class AdditionalServiceGeneration
    {
        public static List<AdditionalService> Generate(RepairDbContext context)
        {
            if (context.AdditionalServices.Any()) return context.AdditionalServices.ToList();

            var names = new[] {
                "Express Repair", "Cleaning", "Diagnostics", "Data Backup"
            };

            var services = names.Select(name => new AdditionalService
            {
                Name = name,
                Price = new Faker().Random.Decimal(100, 800)
            }).ToList();

            context.AddRange(services);
            context.SaveChanges();
            return services;
        }
    }
}
