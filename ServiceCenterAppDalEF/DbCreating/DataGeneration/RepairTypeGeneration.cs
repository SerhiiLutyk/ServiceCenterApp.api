using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.DbCreating;

namespace ServiceCenterAppDalEF.DbCreating.DataGeneration
{
    public class RepairTypeGeneration
    {
        public static List<RepairType> Generate(RepairDbContext context)
        {
            if (context.RepairTypes.Any()) return context.RepairTypes.ToList();

            var repairTypes = new List<RepairType>
            {
                new RepairType { Name = "Ремонт ноутбука", Price = 500 },
                new RepairType { Name = "Ремонт комп'ютера", Price = 400 },
                new RepairType { Name = "Ремонт принтера", Price = 300 },
                new RepairType { Name = "Ремонт монітора", Price = 250 },
                new RepairType { Name = "Ремонт планшета", Price = 350 },
                new RepairType { Name = "Ремонт смартфона", Price = 450 }
            };

            context.RepairTypes.AddRange(repairTypes);
            context.SaveChanges();
            return repairTypes;
        }
    }
}
