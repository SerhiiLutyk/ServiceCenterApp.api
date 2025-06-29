using AutoMapper;
using BLL.Interfaces;
using ServiceCenterAppBLL.DTO.ClientDto;
using ServiceCenterAppBLL.Exceptions;
using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.Interfaces;

namespace ServiceCenterAppBLL.Services;

public class ClientService : IClientService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public ClientService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<IEnumerable<ClientResponseDto>> GetAllAsync(CancellationToken ct = default)
        => _mapper.Map<IEnumerable<ClientResponseDto>>(await _uow.Clients.GetAllAsync(ct));

    public async Task<ClientResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Clients.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Client", id);
        return _mapper.Map<ClientResponseDto>(entity);
    }

    public async Task<ClientResponseDto> CreateAsync(ClientCreateDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<Client>(dto);
        await _uow.Clients.AddAsync(entity, ct);
        await _uow.SaveAsync(ct);
        return _mapper.Map<ClientResponseDto>(entity);
    }

    public async Task<ClientResponseDto> UpdateAsync(int id, ClientUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Clients.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Client", id);

        _mapper.Map(dto, entity);
        _uow.Clients.Update(entity);
        await _uow.SaveAsync(ct);
        return _mapper.Map<ClientResponseDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Clients.GetByIdAsync(id, ct);
        if (entity == null) return false;

        _uow.Clients.Delete(entity);
        await _uow.SaveAsync(ct);
        return true;
    }
}
