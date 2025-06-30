using Microsoft.EntityFrameworkCore;
using ServiceCenterAppDalEF.DbCreating;
using ServiceCenterAppDalEF.Repositories;
using ServiceCenterAppDalEF.Repositories.Interfaces;
using ServiceCenterAppDalEF.Entities;
using System.Collections.Generic;
using System.Threading;

namespace ServiceCenterAppDalEF.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(RepairDbContext context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId, CancellationToken ct = default)
        {
            return await dbSet
                .Include(p => p.Order)
                .Where(p => p.OrderId == orderId)
                .ToListAsync(ct);
        }
    }
}
