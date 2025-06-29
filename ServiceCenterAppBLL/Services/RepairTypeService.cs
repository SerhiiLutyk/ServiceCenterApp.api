using AutoMapper;
using BLL.Interfaces;
using ServiceCenterAppBLL.DTO.RepairTypeDto;
using ServiceCenterAppBLL.Exceptions;
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

    public async Task<IEnumerable<RepairTypeResponseDto>> GetAllAsync(CancellationToken ct = default)
        => _mapper.Map<IEnumerable<RepairTypeResponseDto>>(await _uow.RepairTypes.GetAllAsync(ct));

    public async Task<RepairTypeResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        => _mapper.Map<RepairTypeResponseDto>(
               await _uow.RepairTypes.GetByIdAsync(id, ct) ?? throw new NotFoundException("RepairType", id));

    public async Task<RepairTypeResponseDto> CreateAsync(RepairTypeCreateDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<RepairType>(dto);
        await _uow.RepairTypes.AddAsync(entity, ct);
        await _uow.SaveAsync(ct);
        return _mapper.Map<RepairTypeResponseDto>(entity);
    }

    public async Task<RepairTypeResponseDto> UpdateAsync(int id, RepairTypeUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.RepairTypes.GetByIdAsync(id, ct) ?? throw new NotFoundException("RepairType", id);
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
}
