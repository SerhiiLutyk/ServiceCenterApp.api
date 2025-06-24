using ServiceCenterAppBLL.DTO.ClientDto;

namespace BLL.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ClientResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ClientResponseDto> CreateAsync(ClientCreateDto dto, CancellationToken cancellationToken = default);
        Task<ClientResponseDto?> UpdateAsync(int id, ClientUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
