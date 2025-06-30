using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceCenterAppDalEF.Repositories.Interfaces;

namespace ServiceCenterAppDalEF.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClientRepository Clients { get; }
        IOrderRepository Orders { get; }
        IRepairTypeRepository RepairTypes { get; }
        IAdditionalServiceRepository AdditionalServices { get; }
        IPaymentRepository Payments { get; }

        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}
