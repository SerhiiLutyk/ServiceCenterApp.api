using ServiceCenterAppBLL.DTO.PaymentDto;
using ServiceCenterAppBLL.Pagination;

namespace ServiceCenterAppBLL.Interfaces
{
    public interface IPaymentService
    {
        Task<PagedList<PaymentResponseDto>> GetAllAsync(int page = 1, int pageSize = 10, DateTime? fromDate = null, DateTime? toDate = null, string? paymentMethod = null, string? sortBy = null, string? sortOrder = "asc", CancellationToken cancellationToken = default);
        Task<PaymentResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PaymentResponseDto> CreateAsync(PaymentCreateDto dto, CancellationToken cancellationToken = default);
        Task<PaymentResponseDto?> UpdateAsync(int id, PaymentUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByOrderAsync(int orderId, CancellationToken cancellationToken = default);
        Task<object> GetPaymentStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
        Task<decimal> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
        Task<object> GetRevenueReportAsync(DateTime fromDate, DateTime toDate, string? groupBy = "day", CancellationToken cancellationToken = default);
        Task<object> GetPaymentMethodsReportAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    }
}
