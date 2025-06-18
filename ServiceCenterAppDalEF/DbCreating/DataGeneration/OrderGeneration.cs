using Bogus;
using ServiceCenterAppDalEF.Entities;

namespace RepairServiceDAL.DbCreating.DataGeneration
{
    public class OrderGeneration
    {
        public static List<Order> Generate(
            RepairDbContext context,
            List<Client> clients,
            List<RepairType> repairTypes,
            List<AdditionalService> services)
        {
            if (context.Orders.Any()) return context.Orders.ToList();

            var faker = new Faker("uk");
            var orders = new List<Order>();
            var random = new Random();

            for (int i = 0; i < 15; i++)
            {
                orders.Add(new Order
                {
                    ClientId = faker.PickRandom(clients).ClientId,
                    RepairTypeId = faker.PickRandom(repairTypes).RepairTypeId,
                    AdditionalServiceId = faker.PickRandom(services).ServiceId,
                    OrderDate = faker.Date.Recent(14),
                    Status = faker.PickRandom(new[] { "In Progress", "Done", "Waiting" }),
                    Description = faker.Lorem.Sentence()
                });
            }

            context.Orders.AddRange(orders);
            context.SaveChanges();
            return orders;
        }
    }
}
