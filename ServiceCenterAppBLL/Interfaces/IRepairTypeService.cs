using ServiceCenterAppBLL.DTO.RepairTypeDto;
using ServiceCenterAppBLL.Pagination;

namespace ServiceCenterAppBLL.Interfaces
{
    public interface IRepairTypeService
    {
        Task<PagedList<RepairTypeResponseDto>> GetAllAsync(int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, string? sortOrder = "asc", CancellationToken cancellationToken = default);
        Task<RepairTypeResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<RepairTypeResponseDto> CreateAsync(RepairTypeCreateDto dto, CancellationToken cancellationToken = default);
        Task<RepairTypeResponseDto?> UpdateAsync(int id, RepairTypeUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RepairTypeResponseDto>> GetRepairTypeOrdersAsync(int id, CancellationToken cancellationToken = default);
    }
}
