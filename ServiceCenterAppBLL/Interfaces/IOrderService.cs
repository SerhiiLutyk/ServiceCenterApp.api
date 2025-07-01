using ServiceCenterAppBLL.DTO.OrderDto;
using ServiceCenterAppBLL.Pagination;

namespace ServiceCenterAppBLL.Interfaces
{
    public interface IOrderService
    {
        Task<PagedList<OrderResponseDto>> GetAllAsync(int page = 1, int pageSize = 10, string? status = null, DateTime? fromDate = null, DateTime? toDate = null, string? sortBy = null, string? sortOrder = "asc", CancellationToken cancellationToken = default);
        Task<OrderResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<OrderResponseDto> CreateAsync(OrderCreateDto dto, CancellationToken cancellationToken = default);
        Task<OrderResponseDto?> UpdateAsync(int id, OrderUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<OrderResponseDto>> GetOrderPaymentsAsync(int id, CancellationToken cancellationToken = default);
        Task<OrderResponseDto?> UpdateStatusAsync(int id, string status, CancellationToken cancellationToken = default);
        Task<PagedList<OrderResponseDto>> GetByStatusAsync(string status, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<int> GetTotalOrdersCountAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
        Task<int> GetCompletedOrdersCountAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
        Task<int> GetPendingOrdersCountAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
        Task<decimal> GetAverageOrderValueAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
        Task<object> GetOrdersReportAsync(DateTime fromDate, DateTime toDate, string? status = null, CancellationToken cancellationToken = default);
        Task<object> GetPopularRepairTypesReportAsync(DateTime? fromDate = null, DateTime? toDate = null, int top = 10, CancellationToken cancellationToken = default);
    }
}
