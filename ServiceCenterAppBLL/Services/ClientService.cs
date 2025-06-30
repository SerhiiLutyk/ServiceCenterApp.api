using AutoMapper;
using ServiceCenterAppBLL.Interfaces;
using ServiceCenterAppBLL.DTO.ClientDto;
using ServiceCenterAppBLL.DTO.OrderDto;
using ServiceCenterAppBLL.Exceptions;
using ServiceCenterAppBLL.Pagination;
using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.Interfaces;

namespace ServiceCenterAppBLL.Services;

public class ClientService : IClientService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public ClientService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<PagedList<ClientResponseDto>> GetAllAsync(int page = 1, int pageSize = 10, string? searchTerm = null, CancellationToken ct = default)
    {
        var query = _uow.Clients.GetAll();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x => x.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   x.Phone.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
        var totalCount = query.Count();
        var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var dtos = _mapper.Map<IEnumerable<ClientResponseDto>>(items);
        return new PagedList<ClientResponseDto>(dtos.ToList(), totalCount, page, pageSize);
    }

    public async Task<ClientResponseDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Clients.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Client", id);
        return _mapper.Map<ClientResponseDto>(entity);
    }

    public async Task<ClientResponseDto> CreateAsync(ClientCreateDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<Client>(dto);
        await _uow.Clients.AddAsync(entity, ct);
        await _uow.SaveAsync(ct);
        return _mapper.Map<ClientResponseDto>(entity);
    }

    public async Task<ClientResponseDto?> UpdateAsync(int id, ClientUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Clients.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Client", id);
        _mapper.Map(dto, entity);
        _uow.Clients.Update(entity);
        await _uow.SaveAsync(ct);
        return _mapper.Map<ClientResponseDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Clients.GetByIdAsync(id, ct);
        if (entity == null) return false;
        _uow.Clients.Delete(entity);
        await _uow.SaveAsync(ct);
        return true;
    }

    public async Task<IEnumerable<OrderResponseDto>> GetClientOrdersAsync(int clientId, CancellationToken ct = default)
    {
        var orders = await _uow.Orders.GetByClientIdAsync(clientId);
        return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
    }

    public async Task<int> GetTotalClientsCountAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken ct = default)
    {
        var query = await _uow.Clients.GetAllAsync(ct);
        
        if (fromDate.HasValue)
            query = query.Where(c => c.Orders.Any(o => o.OrderDate >= fromDate));
        
        if (toDate.HasValue)
            query = query.Where(c => c.Orders.Any(o => o.OrderDate <= toDate));
        
        return query.Count();
    }

    public async Task<object> GetClientsReportAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken ct = default)
    {
        var query = await _uow.Clients.GetAllAsync(ct);
        
        if (fromDate.HasValue)
            query = query.Where(c => c.Orders.Any(o => o.OrderDate >= fromDate));
        
        if (toDate.HasValue)
            query = query.Where(c => c.Orders.Any(o => o.OrderDate <= toDate));
        
        var report = new
        {
            TotalClients = query.Count(),
            ClientsWithOrders = query.Count(c => c.Orders.Any()),
            AverageOrdersPerClient = query.Any() ? query.Average(c => c.Orders.Count) : 0
        };
        
        return report;
    }
}
