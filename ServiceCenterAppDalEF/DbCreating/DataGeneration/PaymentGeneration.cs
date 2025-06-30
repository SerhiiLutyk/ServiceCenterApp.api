using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.DbCreating;

namespace ServiceCenterAppDalEF.DbCreating.DataGeneration
{
    public class PaymentGeneration
    {
        public static List<Payment> Generate(RepairDbContext context, List<Order> orders)
        {
            if (context.Payments.Any()) return context.Payments.ToList();

            var payments = new List<Payment>();
            var random = new Random();
            var paymentMethods = new[] { "Cash", "Card", "Bank Transfer" };

            foreach (var order in orders.Where(o => o.Status == "Completed"))
            {
                var payment = new Payment
                {
                    OrderId = order.OrderId,
                    Amount = random.Next(200, 1000),
                    PaymentDate = order.OrderDate.Value.AddDays(random.Next(1, 7)),
                    PaymentMethod = paymentMethods[random.Next(paymentMethods.Length)]
                };
                payments.Add(payment);
            }

            context.Payments.AddRange(payments);
            context.SaveChanges();
            return payments;
        }
    }
}
