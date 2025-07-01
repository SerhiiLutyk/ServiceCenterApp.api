using AutoMapper;
using ServiceCenterAppBLL.Interfaces;
using ServiceCenterAppBLL.DTO.PaymentDto;
using ServiceCenterAppBLL.Exceptions;
using ServiceCenterAppBLL.Pagination;
using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.Interfaces;

namespace ServiceCenterAppBLL.Services;

public class PaymentService : IPaymentService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public PaymentService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<PagedList<PaymentResponseDto>> GetAllAsync(int page = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? paymentMethod = null, string? sortBy = null, string? sortOrder = "asc", CancellationToken ct = default)
    {
        var query = await _uow.Payments.GetAllAsync(ct);
        
        if (fromDate.HasValue)
            query = query.Where(p => p.PaymentDate >= fromDate);
        
        if (toDate.HasValue)
            query = query.Where(p => p.PaymentDate <= toDate);
        
        if (!string.IsNullOrEmpty(paymentMethod))
            query = query.Where(p => p.PaymentMethod == paymentMethod);

        if (!string.IsNullOrEmpty(sortBy))
        {
            switch (sortBy.ToLower())
            {
                case "paymentdate":
                    query = sortOrder == "desc" ? query.OrderByDescending(x => x.PaymentDate) : query.OrderBy(x => x.PaymentDate);
                    break;
                case "amount":
                    query = sortOrder == "desc" ? query.OrderByDescending(x => x.Amount) : query.OrderBy(x => x.Amount);
                    break;
                case "paymentmethod":
                    query = sortOrder == "desc" ? query.OrderByDescending(x => x.PaymentMethod) : query.OrderBy(x => x.PaymentMethod);
                    break;
                default:
                    query = query.OrderBy(x => x.PaymentId);
                    break;
            }
        }
        else
        {
            query = query.OrderBy(x => x.PaymentId);
        }

        var totalCount = query.Count();
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = _mapper.Map<IEnumerable<PaymentResponseDto>>(items);
        
        return new PagedList<PaymentResponseDto>(dtos.ToList(), totalCount, page, pageSize);
    }

    public async Task<PaymentResponseDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Payments.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Payment", id);
        return _mapper.Map<PaymentResponseDto>(entity);
    }

    public async Task<PaymentResponseDto> CreateAsync(PaymentCreateDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<Payment>(dto);
        entity.PaymentDate = DateTime.UtcNow;
        
        await _uow.Payments.AddAsync(entity, ct);
        await _uow.SaveAsync(ct);
        return _mapper.Map<PaymentResponseDto>(entity);
    }

    public async Task<PaymentResponseDto?> UpdateAsync(int id, PaymentUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Payments.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Payment", id);

        _mapper.Map(dto, entity);
        _uow.Payments.Update(entity);
        await _uow.SaveAsync(ct);
        return _mapper.Map<PaymentResponseDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Payments.GetByIdAsync(id, ct);
        if (entity == null) return false;

        _uow.Payments.Delete(entity);
        await _uow.SaveAsync(ct);
        return true;
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByOrderAsync(int orderId, CancellationToken ct = default)
    {
        var payments = await _uow.Payments.GetByOrderIdAsync(orderId, ct);
        return _mapper.Map<IEnumerable<PaymentResponseDto>>(payments);
    }

    public async Task<object> GetPaymentStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken ct = default)
    {
        var query = await _uow.Payments.GetAllAsync(ct);
        
        if (fromDate.HasValue)
            query = query.Where(p => p.PaymentDate >= fromDate);
        
        if (toDate.HasValue)
            query = query.Where(p => p.PaymentDate <= toDate);
        
        var statistics = new
        {
            TotalPayments = query.Count(),
            TotalAmount = query.Sum(p => p.Amount ?? 0),
            AverageAmount = query.Any() ? query.Average(p => p.Amount ?? 0) : 0,
            PaymentsByMethod = query.GroupBy(p => p.PaymentMethod).Select(g => new { Method = g.Key, Count = g.Count(), Amount = g.Sum(p => p.Amount ?? 0) })
        };
        
        return statistics;
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken ct = default)
    {
        var query = await _uow.Payments.GetAllAsync(ct);
        
        if (fromDate.HasValue)
            query = query.Where(p => p.PaymentDate >= fromDate);
        
        if (toDate.HasValue)
            query = query.Where(p => p.PaymentDate <= toDate);
        
        return query.Sum(p => p.Amount ?? 0);
    }

    public async Task<object> GetRevenueReportAsync(DateTime fromDate, DateTime toDate, string? groupBy = "day", CancellationToken ct = default)
    {
        var query = await _uow.Payments.GetAllAsync(ct);
        query = query.Where(p => p.PaymentDate >= fromDate && p.PaymentDate <= toDate);
        
        var report = new
        {
            TotalRevenue = query.Sum(p => p.Amount ?? 0),
            TotalPayments = query.Count(),
            RevenueByMethod = query.GroupBy(p => p.PaymentMethod).Select(g => new { Method = g.Key, Count = g.Count(), Amount = g.Sum(p => p.Amount ?? 0) })
        };
        
        return report;
    }

    public async Task<object> GetPaymentMethodsReportAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken ct = default)
    {
        var query = await _uow.Payments.GetAllAsync(ct);
        
        if (fromDate.HasValue)
            query = query.Where(p => p.PaymentDate >= fromDate);
        
        if (toDate.HasValue)
            query = query.Where(p => p.PaymentDate <= toDate);
        
        var report = query
            .GroupBy(p => p.PaymentMethod)
            .Select(g => new { Method = g.Key, Count = g.Count(), Amount = g.Sum(p => p.Amount ?? 0) })
            .OrderByDescending(x => x.Amount);
        
        return report;
    }
}
