using ServiceCenterAppDalEF.DbCreating;

namespace ServiceCenterAppDalEF.DbCreating.DataGeneration
{
    public class DataGenerator
    {
        public static void GenerateAll(RepairDbContext context)
        {
            var clients = ClientGeneration.Generate(context);
            var repairTypes = RepairTypeGeneration.Generate(context);
            var additionalServices = AdditionalServiceGeneration.Generate(context);
            var orders = OrderGeneration.Generate(context, clients, repairTypes, additionalServices);
            PaymentGeneration.Generate(context, orders);
        }
    }
}
