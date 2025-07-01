using AutoMapper;
using ServiceCenterAppBLL.Interfaces;
using ServiceCenterAppBLL.DTO.RepairTypeDto;
using ServiceCenterAppBLL.Exceptions;
using ServiceCenterAppBLL.Pagination;
using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.Interfaces;

namespace ServiceCenterAppBLL.Services;

public class RepairTypeService : IRepairTypeService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public RepairTypeService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<PagedList<RepairTypeResponseDto>> GetAllAsync(int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, string? sortOrder = "asc", CancellationToken ct = default)
    {
        var query = await _uow.RepairTypes.GetAllAsync(ct);
        
        if (!string.IsNullOrEmpty(searchTerm))
        {
            int idValue;
            bool isId = int.TryParse(searchTerm, out idValue);
            query = query.Where(x =>
                x.Name.ToLower().Contains(searchTerm.ToLower())
                || (isId && x.RepairTypeId == idValue)
            );
        }

        // Сортування
        if (!string.IsNullOrEmpty(sortBy))
        {
            switch (sortBy.ToLower())
            {
                case "name":
                    query = sortOrder == "desc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                    break;
                default:
                    query = query.OrderBy(x => x.RepairTypeId);
                    break;
            }
        }
        else
        {
            query = query.OrderBy(x => x.RepairTypeId);
        }

        var totalCount = query.Count();
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = _mapper.Map<IEnumerable<RepairTypeResponseDto>>(items);
        
        return new PagedList<RepairTypeResponseDto>(dtos.ToList(), totalCount, page, pageSize);
    }

    public async Task<RepairTypeResponseDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.RepairTypes.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("RepairType", id);
        return _mapper.Map<RepairTypeResponseDto>(entity);
    }

    public async Task<RepairTypeResponseDto> CreateAsync(RepairTypeCreateDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<RepairType>(dto);
        await _uow.RepairTypes.AddAsync(entity, ct);
        await _uow.SaveAsync(ct);
        return _mapper.Map<RepairTypeResponseDto>(entity);
    }

    public async Task<RepairTypeResponseDto?> UpdateAsync(int id, RepairTypeUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.RepairTypes.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("RepairType", id);

        _mapper.Map(dto, entity);
        _uow.RepairTypes.Update(entity);
        await _uow.SaveAsync(ct);
        return _mapper.Map<RepairTypeResponseDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.RepairTypes.GetByIdAsync(id, ct);
        if (entity == null) return false;

        _uow.RepairTypes.Delete(entity);
        await _uow.SaveAsync(ct);
        return true;
    }

    public async Task<IEnumerable<RepairTypeResponseDto>> GetRepairTypeOrdersAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.RepairTypes.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("RepairType", id);
        
        // Тут можна додати логіку для отримання замовлень для конкретного типу ремонту
        // Поки що повертаємо порожній список
        return new List<RepairTypeResponseDto>();
    }
}
