using Microsoft.EntityFrameworkCore;
using RepairServiceDAL.DbCreating;
using RepairServiceDAL.Repositories;
using RepairServiceDAL.Repositories.Interfaces;
using ServiceCenterAppDalEF.Entities;
using System.Collections.Generic;

namespace ServiceCenterAppDalEF.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(RepairDbContext context) : base(context) { }

        public async Task<List<Order>> GetByClientIdAsync(int clientId)
        {
            return await dbSet
                .Where(o => o.ClientId == clientId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}
