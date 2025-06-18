using ServiceCenterAppDalEF.Entities;

namespace RepairServiceDAL.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<List<Payment>> GetByOrderIdAsync(int orderId);
    }
}
