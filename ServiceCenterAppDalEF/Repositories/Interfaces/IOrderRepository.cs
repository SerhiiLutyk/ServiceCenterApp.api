using ServiceCenterAppDalEF.Entities;

namespace ServiceCenterAppDalEF.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetByClientIdAsync(int clientId);
    }
}
