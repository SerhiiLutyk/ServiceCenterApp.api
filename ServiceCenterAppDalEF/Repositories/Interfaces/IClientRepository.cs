using ServiceCenterAppDalEF.Entities;

namespace ServiceCenterAppDalEF.Repositories.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client?> GetByPhoneAsync(string phone);
    }
}
