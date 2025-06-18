using Microsoft.EntityFrameworkCore;
using RepairServiceDAL.DbCreating;
using RepairServiceDAL.Repositories;
using RepairServiceDAL.Repositories.Interfaces;
using ServiceCenterAppDalEF.Entities;
using System.Collections.Generic;

namespace ServiceCenterAppDalEF.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(RepairDbContext context) : base(context) { }

        public async Task<Client?> GetByPhoneAsync(string phone)
        {
            return await dbSet.FirstOrDefaultAsync(c => c.Phone == phone);
        }
    }
}
