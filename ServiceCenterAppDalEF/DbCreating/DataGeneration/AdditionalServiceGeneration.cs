using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.DbCreating;

namespace ServiceCenterAppDalEF.DbCreating.DataGeneration
{
    public class AdditionalServiceGeneration
    {
        public static List<AdditionalService> Generate(RepairDbContext context)
        {
            if (context.AdditionalServices.Any()) return context.AdditionalServices.ToList();

            var services = new List<AdditionalService>
            {
                new AdditionalService { Name = "Чистка від пилу", Price = 200 },
                new AdditionalService { Name = "Заміна термопасти", Price = 300 },
                new AdditionalService { Name = "Встановлення додаткового ПО", Price = 150 },
                new AdditionalService { Name = "Резервне копіювання даних", Price = 250 },
                new AdditionalService { Name = "Налаштування мережі", Price = 180 }
            };

            context.AdditionalServices.AddRange(services);
            context.SaveChanges();
            return services;
        }
    }
}
