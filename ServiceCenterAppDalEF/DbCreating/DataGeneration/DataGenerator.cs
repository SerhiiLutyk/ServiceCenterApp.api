namespace RepairServiceDAL.DbCreating.DataGeneration
{
    public static class DataGenerator
    {
        public static void Initialize(RepairDbContext context)
        {
            var clients = ClientGeneration.Generate(context);
            var repairTypes = RepairTypeGeneration.Generate(context);
            var services = AdditionalServiceGeneration.Generate(context);
            var orders = OrderGeneration.Generate(context, clients, repairTypes, services);
            var payments = PaymentGeneration.Generate(context, orders);
        }
    }
}
