using AutoMapper;
using ServiceCenterAppBLL.Interfaces;
using ServiceCenterAppBLL.DTO.AdditionalServiceDto;
using ServiceCenterAppBLL.Exceptions;
using ServiceCenterAppBLL.Pagination;
using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.Interfaces;

namespace ServiceCenterAppBLL.Services;

public class AdditionalServiceService : IAdditionalServiceService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public AdditionalServiceService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<PagedList<AdditionalServiceResponseDto>> GetAllAsync(int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, string? sortOrder = "asc", CancellationToken ct = default)
    {
        var query = await _uow.AdditionalServices.GetAllAsync(ct);
        
        if (!string.IsNullOrEmpty(searchTerm))
        {
            int idValue;
            bool isId = int.TryParse(searchTerm, out idValue);
            query = query.Where(x =>
                x.Name.ToLower().Contains(searchTerm.ToLower())
                || (isId && x.ServiceId == idValue)
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
                    query = query.OrderBy(x => x.ServiceId);
                    break;
            }
        }
        else
        {
            query = query.OrderBy(x => x.ServiceId);
        }

        var totalCount = query.Count();
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = _mapper.Map<IEnumerable<AdditionalServiceResponseDto>>(items);
        
        return new PagedList<AdditionalServiceResponseDto>(dtos.ToList(), totalCount, page, pageSize);
    }

    public async Task<AdditionalServiceResponseDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.AdditionalServices.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("AdditionalService", id);
        return _mapper.Map<AdditionalServiceResponseDto>(entity);
    }

    public async Task<AdditionalServiceResponseDto> CreateAsync(AdditionalServiceCreateDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<AdditionalService>(dto);
        await _uow.AdditionalServices.AddAsync(entity, ct);
        await _uow.SaveAsync(ct);
        return _mapper.Map<AdditionalServiceResponseDto>(entity);
    }

    public async Task<AdditionalServiceResponseDto?> UpdateAsync(int id, AdditionalServiceUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.AdditionalServices.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("AdditionalService", id);

        _mapper.Map(dto, entity);
        _uow.AdditionalServices.Update(entity);
        await _uow.SaveAsync(ct);
        return _mapper.Map<AdditionalServiceResponseDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.AdditionalServices.GetByIdAsync(id, ct);
        if (entity == null) return false;

        _uow.AdditionalServices.Delete(entity);
        await _uow.SaveAsync(ct);
        return true;
    }

    public async Task<IEnumerable<AdditionalServiceResponseDto>> GetAdditionalServiceOrdersAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.AdditionalServices.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("AdditionalService", id);
        
        // Тут можна додати логіку для отримання замовлень для додаткової послуги
        // Поки що повертаємо порожній список
        return new List<AdditionalServiceResponseDto>();
    }
}
