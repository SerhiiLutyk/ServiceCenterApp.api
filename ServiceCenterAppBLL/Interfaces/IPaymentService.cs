using ServiceCenterAppBLL.DTO.PaymentDto;

namespace BLL.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<PaymentResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<PaymentResponseDto> CreateAsync(PaymentCreateDto dto, CancellationToken cancellationToken = default);
        Task<PaymentResponseDto?> UpdateAsync(int id, PaymentUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentResponseDto>> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
    }
}
