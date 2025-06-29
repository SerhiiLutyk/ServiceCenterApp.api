using AutoMapper;
using BLL.Interfaces;
using ServiceCenterAppBLL.DTO.AdditionalServiceDto;
using ServiceCenterAppBLL.Exceptions;
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

    public async Task<IEnumerable<AdditionalServiceResponseDto>> GetAllAsync(CancellationToken ct = default)
        => _mapper.Map<IEnumerable<AdditionalServiceResponseDto>>(await _uow.AdditionalServices.GetAllAsync(ct));

    public async Task<AdditionalServiceResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        => _mapper.Map<AdditionalServiceResponseDto>(
               await _uow.AdditionalServices.GetByIdAsync(id, ct) ?? throw new NotFoundException("AdditionalService", id));

    public async Task<AdditionalServiceResponseDto> CreateAsync(AdditionalServiceCreateDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<AdditionalService>(dto);
        await _uow.AdditionalServices.AddAsync(entity, ct);
        await _uow.SaveAsync(ct);
        return _mapper.Map<AdditionalServiceResponseDto>(entity);
    }

    public async Task<AdditionalServiceResponseDto> UpdateAsync(int id, AdditionalServiceUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.AdditionalServices.GetByIdAsync(id, ct) ?? throw new NotFoundException("AdditionalService", id);
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
}
