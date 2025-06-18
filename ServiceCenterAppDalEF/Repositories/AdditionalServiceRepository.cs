using RepairServiceDAL.DbCreating;
using RepairServiceDAL.Repositories.Interfaces;
using ServiceCenterAppDalEF.Entities;

namespace RepairServiceDAL.Repositories
{
    public class AdditionalServiceRepository : GenericRepository<AdditionalService>, IAdditionalServiceRepository
    {
        public AdditionalServiceRepository(RepairDbContext context) : base(context) { }
    }
}
