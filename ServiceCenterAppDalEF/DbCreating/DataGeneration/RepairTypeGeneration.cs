using Bogus;
using ServiceCenterAppDalEF.Entities;

namespace RepairServiceDAL.DbCreating.DataGeneration
{
    public class RepairTypeGeneration
    {
        public static List<RepairType> Generate(RepairDbContext context)
        {
            if (context.RepairTypes.Any()) return context.RepairTypes.ToList();

            var names = new[] {
                "Screen Replacement", "Battery Swap", "Board Repair",
                "Water Damage Cleanup", "Camera Fix", "Software Flash"
            };

            var repairTypes = names.Select(name => new RepairType
            {
                Name = name,
                Price = new Faker().Random.Decimal(500, 2500)
            }).ToList();

            context.RepairTypes.AddRange(repairTypes);
            context.SaveChanges();
            return repairTypes;
        }
    }
}
