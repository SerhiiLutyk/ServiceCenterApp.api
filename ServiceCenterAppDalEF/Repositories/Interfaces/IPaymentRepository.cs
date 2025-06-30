using ServiceCenterAppDalEF.Entities;

namespace ServiceCenterAppDalEF.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId, CancellationToken ct = default);
    }
}
