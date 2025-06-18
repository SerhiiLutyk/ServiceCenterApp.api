using Bogus;
using ServiceCenterAppDalEF.Entities;

namespace RepairServiceDAL.DbCreating.DataGeneration
{
    public class PaymentGeneration
    {
        public static List<Payment> Generate(RepairDbContext context, List<Order> orders)
        {
            if (context.Payments.Any()) return context.Payments.ToList();

            var faker = new Faker("uk");
            var payments = new List<Payment>();

            foreach (var order in orders)
            {
                payments.Add(new Payment
                {
                    OrderId = order.OrderId,
                    Amount = faker.Random.Decimal(700, 3000),
                    PaymentDate = order.OrderDate?.AddDays(1),
                    PaymentMethod = faker.PickRandom(new[] { "Card", "Cash" })
                });
            }

            context.Payments.AddRange(payments);
            context.SaveChanges();
            return payments;
        }
    }
}
