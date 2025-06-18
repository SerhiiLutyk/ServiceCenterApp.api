using Bogus;
using ServiceCenterAppDalEF.Entities;

namespace RepairServiceDAL.DbCreating.DataGeneration
{
    public class ClientGeneration
    {
        public static List<Client> Generate(RepairDbContext context)
        {
            if (context.Clients.Any()) return context.Clients.ToList();

            var faker = new Faker<Client>("uk")
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("+380#########"));

            var clients = faker.Generate(10);

            context.Clients.AddRange(clients);
            context.SaveChanges();
            return clients;
        }
    }
}
