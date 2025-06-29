using RepairServiceDAL.Repositories;
using RepairServiceDAL.Repositories.Interfaces;
using ServiceCenterAppDalEF.Interfaces;
using ServiceCenterAppDalEF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairServiceDAL.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepairDbContext _context;

        public UnitOfWork(RepairDbContext context)
        {
            _context = context;
        }

        public IClientRepository Clients => new ClientRepository(_context);
        public IOrderRepository Orders => new OrderRepository(_context);
        public IRepairTypeRepository RepairTypes => new RepairTypeRepository(_context);
        public IAdditionalServiceRepository AdditionalServices => new AdditionalServiceRepository(_context);
        public IPaymentRepository Payments => new PaymentRepository(_context);

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose() => _context.Dispose();

    }
}
