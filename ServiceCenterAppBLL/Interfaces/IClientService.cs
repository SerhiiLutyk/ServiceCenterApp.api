using ServiceCenterAppBLL.DTO.ClientDto;
using ServiceCenterAppBLL.Pagination;
using ServiceCenterAppBLL.DTO.OrderDto;

namespace ServiceCenterAppBLL.Interfaces
{
    public interface IClientService
    {
        Task<PagedList<ClientResponseDto>> GetAllAsync(int page = 1, int pageSize = 10, string? searchTerm = null, CancellationToken cancellationToken = default);
        Task<ClientResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ClientResponseDto> CreateAsync(ClientCreateDto dto, CancellationToken cancellationToken = default);
        Task<ClientResponseDto?> UpdateAsync(int id, ClientUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<OrderResponseDto>> GetClientOrdersAsync(int clientId, CancellationToken cancellationToken = default);
        Task<int> GetTotalClientsCountAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
        Task<object> GetClientsReportAsync(DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    }
}
