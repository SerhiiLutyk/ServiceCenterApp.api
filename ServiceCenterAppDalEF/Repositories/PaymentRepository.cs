using Microsoft.EntityFrameworkCore;
using RepairServiceDAL.DbCreating;
using RepairServiceDAL.Repositories.Interfaces;
using ServiceCenterAppDalEF.Entities;
using System.Collections.Generic;

namespace RepairServiceDAL.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(RepairDbContext context) : base(context) { }

        public async Task<List<Payment>> GetByOrderIdAsync(int orderId)
        {
            return await dbSet
                .Where(p => p.OrderId == orderId)
                .OrderBy(p => p.PaymentDate)
                .ToListAsync();
        }
    }
}
