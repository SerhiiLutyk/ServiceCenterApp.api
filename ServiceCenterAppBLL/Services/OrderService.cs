using AutoMapper;
using ServiceCenterAppBLL.Interfaces;
using ServiceCenterAppBLL.DTO.OrderDto;
using ServiceCenterAppBLL.Exceptions;
using ServiceCenterAppBLL.Pagination;
using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.Interfaces;

namespace ServiceCenterAppBLL.Services;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public OrderService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<PagedList<OrderResponseDto>> GetAllAsync(int page = 1, int pageSize = 10, string? status = null, DateTime? fromDate = null, DateTime? toDate = null, string? sortBy = null, string? sortOrder = "asc", CancellationToken ct = default)
    {
        var query = await _uow.Orders.GetAllAsync(ct);
        
        if (!string.IsNullOrEmpty(status))
            query = query.Where(o => o.Status == status);
        
        if (fromDate.HasValue)
            query = query.Where(o => o.OrderDate >= fromDate);
        
        if (toDate.HasValue)
            query = query.Where(o => o.OrderDate <= toDate);

        // Сортування
        if (!string.IsNullOrEmpty(sortBy))
        {
            switch (sortBy.ToLower())
            {
                case "orderdate":
                    query = sortOrder == "desc" ? query.OrderByDescending(x => x.OrderDate) : query.OrderBy(x => x.OrderDate);
                    break;
                case "status":
                    query = sortOrder == "desc" ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status);
                    break;
                case "description":
                    query = sortOrder == "desc" ? query.OrderByDescending(x => x.Description) : query.OrderBy(x => x.Description);
                    break;
                default:
                    query = query.OrderBy(x => x.OrderId);
                    break;
            }
        }
        else
        {
            query = query.OrderBy(x => x.OrderId);
        }

        var totalCount = query.Count();
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = _mapper.Map<IEnumerable<OrderResponseDto>>(items);
        
        return new PagedList<OrderResponseDto>(dtos.ToList(), totalCount, page, pageSize);
    }

    public async Task<OrderResponseDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Orders.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Order", id);
        return _mapper.Map<OrderResponseDto>(entity);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetByClientIdAsync(int clientId, CancellationToken ct = default)
    {
        var list = await _uow.Orders.GetByClientIdAsync(clientId);
        return _mapper.Map<IEnumerable<OrderResponseDto>>(list);
    }

    public async Task<OrderResponseDto> CreateAsync(OrderCreateDto dto, CancellationToken ct = default)
    {
        ValidateRelated(dto.ClientId, dto.RepairTypeId, dto.AdditionalServiceId);

        var entity = _mapper.Map<Order>(dto);
        entity.Status = "Pending";
        entity.OrderDate = DateTime.UtcNow;

        await _uow.Orders.AddAsync(entity, ct);
        await _uow.SaveAsync(ct);
        return _mapper.Map<OrderResponseDto>(entity);
    }

    public async Task<OrderResponseDto?> UpdateAsync(int id, OrderUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Orders.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Order", id);

        if (dto.AdditionalServiceId.HasValue)
            EnsureAdditionalExists(dto.AdditionalServiceId.Value);

        _mapper.Map(dto, entity);
        _uow.Orders.Update(entity);
        await _uow.SaveAsync(ct);
        return _mapper.Map<OrderResponseDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Orders.GetByIdAsync(id, ct);
        if (entity == null) return false;

        if (entity.Payments?.Any() == true)
            throw new ConflictException("Cannot delete order with payments.");

        _uow.Orders.Delete(entity);
        await _uow.SaveAsync(ct);
        return true;
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrderPaymentsAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Orders.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Order", id);
        
        // Тут можна додати логіку для отримання платежів замовлення
        return new List<OrderResponseDto>();
    }

    public async Task<OrderResponseDto?> UpdateStatusAsync(int id, string status, CancellationToken ct = default)
    {
        var entity = await _uow.Orders.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Order", id);

        entity.Status = status;
        _uow.Orders.Update(entity);
        await _uow.SaveAsync(ct);
        return _mapper.Map<OrderResponseDto>(entity);
    }

    public async Task<PagedList<OrderResponseDto>> GetByStatusAsync(string status, int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var query = await _uow.Orders.GetAllAsync(ct);
        query = query.Where(o => o.Status == status);

        var totalCount = query.Count();
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = _mapper.Map<IEnumerable<OrderResponseDto>>(items);
        
        return new PagedList<OrderResponseDto>(dtos.ToList(), totalCount, page, pageSize);
    }

    public async Task<int> GetTotalOrdersCountAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken ct = default)
    {
        var query = await _uow.Orders.GetAllAsync(ct);
        
        if (fromDate.HasValue)
            query = query.Where(o => o.OrderDate >= fromDate);
        
        if (toDate.HasValue)
            query = query.Where(o => o.OrderDate <= toDate);
        
        return query.Count();
    }

    public async Task<int> GetCompletedOrdersCountAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken ct = default)
    {
        var query = await _uow.Orders.GetAllAsync(ct);
        query = query.Where(o => o.Status == "Completed");
        
        if (fromDate.HasValue)
            query = query.Where(o => o.OrderDate >= fromDate);
        
        if (toDate.HasValue)
            query = query.Where(o => o.OrderDate <= toDate);
        
        return query.Count();
    }

    public async Task<int> GetPendingOrdersCountAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken ct = default)
    {
        var query = await _uow.Orders.GetAllAsync(ct);
        query = query.Where(o => o.Status == "Pending");
        
        if (fromDate.HasValue)
            query = query.Where(o => o.OrderDate >= fromDate);
        
        if (toDate.HasValue)
            query = query.Where(o => o.OrderDate <= toDate);
        
        return query.Count();
    }

    public async Task<decimal> GetAverageOrderValueAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken ct = default)
    {
        var query = await _uow.Orders.GetAllAsync(ct);
        
        if (fromDate.HasValue)
            query = query.Where(o => o.OrderDate >= fromDate);
        
        if (toDate.HasValue)
            query = query.Where(o => o.OrderDate <= toDate);
        
        // Тут можна додати логіку для розрахунку середньої вартості замовлення
        return query.Any() ? 0 : 0;
    }

    public async Task<object> GetOrdersReportAsync(DateTime fromDate, DateTime toDate, string? status = null, CancellationToken ct = default)
    {
        var query = await _uow.Orders.GetAllAsync(ct);
        query = query.Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate);
        
        if (!string.IsNullOrEmpty(status))
            query = query.Where(o => o.Status == status);
        
        var report = new
        {
            TotalOrders = query.Count(),
            OrdersByStatus = query.GroupBy(o => o.Status).Select(g => new { Status = g.Key, Count = g.Count() }),
            AverageOrdersPerDay = query.Any() ? query.Count() / (toDate - fromDate).Days : 0
        };
        
        return report;
    }

    public async Task<object> GetPopularRepairTypesReportAsync(DateTime? fromDate = null, DateTime? toDate = null, int top = 10, CancellationToken ct = default)
    {
        var query = await _uow.Orders.GetAllAsync(ct);
        
        if (fromDate.HasValue)
            query = query.Where(o => o.OrderDate >= fromDate);
        
        if (toDate.HasValue)
            query = query.Where(o => o.OrderDate <= toDate);
        
        var popularTypes = query
            .GroupBy(o => o.RepairTypeId)
            .Select(g => new { RepairTypeId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(top);
        
        return popularTypes;
    }

    /* helpers */
    private void ValidateRelated(int clientId, int repairId, int? addId)
    {
        if (_uow.Clients.GetByIdAsync(clientId).Result == null)
            throw new ValidationException($"Client {clientId} not exists.");

        if (_uow.RepairTypes.GetByIdAsync(repairId).Result == null)
            throw new ValidationException($"RepairType {repairId} not exists.");

        if (addId.HasValue) EnsureAdditionalExists(addId.Value);
    }

    private void EnsureAdditionalExists(int id)
    {
        if (_uow.AdditionalServices.GetByIdAsync(id).Result == null)
            throw new ValidationException($"AdditionalService {id} not exists.");
    }
}
