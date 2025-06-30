using Microsoft.EntityFrameworkCore;
using ServiceCenterAppDalEF.DbCreating;
using ServiceCenterAppDalEF.Repositories;
using ServiceCenterAppDalEF.Repositories.Interfaces;
using ServiceCenterAppDalEF.Entities;
using System.Collections.Generic;

namespace ServiceCenterAppDalEF.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(RepairDbContext context) : base(context) { }

        public async Task<IEnumerable<Order>> GetByClientIdAsync(int clientId)
        {
            return await dbSet
                .Include(o => o.Client)
                .Include(o => o.RepairType)
                .Include(o => o.AdditionalService)
                .Where(o => o.ClientId == clientId)
                .ToListAsync();
        }
    }
}
