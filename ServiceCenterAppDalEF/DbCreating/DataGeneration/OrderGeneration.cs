using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.DbCreating;

namespace ServiceCenterAppDalEF.DbCreating.DataGeneration
{
    public class OrderGeneration
    {
        public static List<Order> Generate(RepairDbContext context, List<Client> clients, List<RepairType> repairTypes, List<AdditionalService> additionalServices)
        {
            if (context.Orders.Any()) return context.Orders.ToList();

            var orders = new List<Order>();
            var random = new Random();

            foreach (var client in clients)
            {
                var orderCount = random.Next(1, 4);
                for (int i = 0; i < orderCount; i++)
                {
                    var order = new Order
                    {
                        ClientId = client.ClientId,
                        OrderDate = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                        Status = random.Next(0, 3) switch
                        {
                            0 => "Pending",
                            1 => "In Progress",
                            _ => "Completed"
                        },
                        Description = $"Ремонт техніки для клієнта {client.FirstName}",
                        RepairTypeId = repairTypes[random.Next(repairTypes.Count)].RepairTypeId,
                        AdditionalServiceId = random.Next(0, 2) == 1 ? additionalServices[random.Next(additionalServices.Count)].ServiceId : null
                    };
                    orders.Add(order);
                }
            }

            context.Orders.AddRange(orders);
            context.SaveChanges();
            return orders;
        }
    }
}
