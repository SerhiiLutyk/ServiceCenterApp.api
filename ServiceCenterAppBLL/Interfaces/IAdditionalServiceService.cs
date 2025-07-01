using ServiceCenterAppBLL.DTO.AdditionalServiceDto;
using ServiceCenterAppBLL.Pagination;

namespace ServiceCenterAppBLL.Interfaces
{
    public interface IAdditionalServiceService
    {
        Task<PagedList<AdditionalServiceResponseDto>> GetAllAsync(int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, string? sortOrder = "asc", CancellationToken cancellationToken = default);
        Task<AdditionalServiceResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<AdditionalServiceResponseDto> CreateAsync(AdditionalServiceCreateDto dto, CancellationToken cancellationToken = default);
        Task<AdditionalServiceResponseDto?> UpdateAsync(int id, AdditionalServiceUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<AdditionalServiceResponseDto>> GetAdditionalServiceOrdersAsync(int id, CancellationToken cancellationToken = default);
    }
}
