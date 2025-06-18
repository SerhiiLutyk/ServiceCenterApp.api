using ServiceCenterAppDalEF.Entities;

namespace RepairServiceDAL.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetByClientIdAsync(int clientId);
    }
}
