using ServiceCenterAppBLL.DTO.AdditionalServiceDto;

namespace BLL.Interfaces
{
    public interface IAdditionalServiceService
    {
        Task<IEnumerable<AdditionalServiceResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<AdditionalServiceResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<AdditionalServiceResponseDto> CreateAsync(AdditionalServiceCreateDto dto, CancellationToken cancellationToken = default);
        Task<AdditionalServiceResponseDto?> UpdateAsync(int id, AdditionalServiceUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
