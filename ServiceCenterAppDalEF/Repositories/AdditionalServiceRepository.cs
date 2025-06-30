using ServiceCenterAppDalEF.DbCreating;
using ServiceCenterAppDalEF.Repositories;
using ServiceCenterAppDalEF.Repositories.Interfaces;
using ServiceCenterAppDalEF.Entities;

namespace ServiceCenterAppDalEF.Repositories
{
    public class AdditionalServiceRepository : GenericRepository<AdditionalService>, IAdditionalServiceRepository
    {
        public AdditionalServiceRepository(RepairDbContext context) : base(context) { }
    }
}
