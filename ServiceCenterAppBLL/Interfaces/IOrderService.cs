using ServiceCenterAppBLL.DTO.OrderDto;

namespace BLL.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<OrderResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<OrderResponseDto> CreateAsync(OrderCreateDto dto, CancellationToken cancellationToken = default);
        Task<OrderResponseDto?> UpdateAsync(int id, OrderUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<OrderResponseDto>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
    }
}
