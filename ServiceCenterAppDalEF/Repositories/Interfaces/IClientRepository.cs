using ServiceCenterAppDalEF.Entities;

namespace RepairServiceDAL.Repositories.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client?> GetByPhoneAsync(string phone);
    }
}
