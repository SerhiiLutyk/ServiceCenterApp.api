using AutoMapper;
using BLL.Interfaces;
using ServiceCenterAppBLL.DTO.OrderDto;
using ServiceCenterAppBLL.Exceptions;
using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.Interfaces;

namespace ServiceCenterAppBLL.Services;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public OrderService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllAsync(CancellationToken ct = default)
        => _mapper.Map<IEnumerable<OrderResponseDto>>(await _uow.Orders.GetAllAsync(ct));

    public async Task<OrderResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Orders.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Order", id);
        return _mapper.Map<OrderResponseDto>(entity);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetByClientIdAsync(int clientId, CancellationToken ct = default)
    {
        var list = await _uow.Orders.GetByClientIdAsync(clientId);
        return _mapper.Map<IEnumerable<OrderResponseDto>>(list);
    }

    public async Task<OrderResponseDto> CreateAsync(OrderCreateDto dto, CancellationToken ct = default)
    {
        ValidateRelated(dto.ClientId, dto.RepairTypeId, dto.AdditionalServiceId);

        var entity = _mapper.Map<Order>(dto);
        entity.Status = "New";
        entity.OrderDate = DateTime.UtcNow;

        await _uow.Orders.AddAsync(entity, ct);
        await _uow.SaveAsync(ct);
        return _mapper.Map<OrderResponseDto>(entity);
    }

    public async Task<OrderResponseDto> UpdateAsync(int id, OrderUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Orders.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Order", id);

        if (dto.AdditionalServiceId.HasValue)
            EnsureAdditionalExists(dto.AdditionalServiceId.Value);

        _mapper.Map(dto, entity);
        _uow.Orders.Update(entity);
        await _uow.SaveAsync(ct);
        return _mapper.Map<OrderResponseDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Orders.GetByIdAsync(id, ct);
        if (entity == null) return false;

        if (entity.Payments?.Any() == true)
            throw new ConflictException("Cannot delete order with payments.");

        _uow.Orders.Delete(entity);
        await _uow.SaveAsync(ct);
        return true;
    }

    /* helpers */
    private void ValidateRelated(int clientId, int repairId, int? addId)
    {
        if (_uow.Clients.GetByIdAsync(clientId).Result == null)
            throw new ValidationException($"Client {clientId} not exists.");

        if (_uow.RepairTypes.GetByIdAsync(repairId).Result == null)
            throw new ValidationException($"RepairType {repairId} not exists.");

        if (addId.HasValue) EnsureAdditionalExists(addId.Value);
    }

    private void EnsureAdditionalExists(int id)
    {
        if (_uow.AdditionalServices.GetByIdAsync(id).Result == null)
            throw new ValidationException($"AdditionalService {id} not exists.");
    }
}
