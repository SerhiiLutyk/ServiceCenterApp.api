using RepairServiceDAL.DbCreating;
using RepairServiceDAL.Repositories.Interfaces;
using ServiceCenterAppDalEF.Entities;

namespace RepairServiceDAL.Repositories
{
    public class RepairTypeRepository : GenericRepository<RepairType>, IRepairTypeRepository
    {
        public RepairTypeRepository(RepairDbContext context) : base(context) { }
    }
}
