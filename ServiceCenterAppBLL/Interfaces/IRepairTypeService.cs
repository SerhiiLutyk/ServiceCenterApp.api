using ServiceCenterAppBLL.DTO.RepairTypeDto;

namespace BLL.Interfaces
{
    public interface IRepairTypeService
    {
        Task<IEnumerable<RepairTypeResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<RepairTypeResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<RepairTypeResponseDto> CreateAsync(RepairTypeCreateDto dto, CancellationToken cancellationToken = default);
        Task<RepairTypeResponseDto?> UpdateAsync(int id, RepairTypeUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
