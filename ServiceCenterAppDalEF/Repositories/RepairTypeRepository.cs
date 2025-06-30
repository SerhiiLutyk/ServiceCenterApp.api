using ServiceCenterAppDalEF.DbCreating;
using ServiceCenterAppDalEF.Repositories;
using ServiceCenterAppDalEF.Repositories.Interfaces;
using ServiceCenterAppDalEF.Entities;

namespace ServiceCenterAppDalEF.Repositories
{
    public class RepairTypeRepository : GenericRepository<RepairType>, IRepairTypeRepository
    {
        public RepairTypeRepository(RepairDbContext context) : base(context) { }
    }
}
