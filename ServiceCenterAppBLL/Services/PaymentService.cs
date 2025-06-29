using AutoMapper;
using BLL.Interfaces;
using ServiceCenterAppBLL.DTO.PaymentDto;
using ServiceCenterAppBLL.Exceptions;
using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppDalEF.Interfaces;

namespace ServiceCenterAppBLL.Services;

public class PaymentService : IPaymentService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public PaymentService(IMapper mapper, IUnitOfWork uow)
    {
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetAllAsync(CancellationToken ct = default)
        => _mapper.Map<IEnumerable<PaymentResponseDto>>(await _uow.Payments.GetAllAsync(ct));

    public async Task<PaymentResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        => _mapper.Map<PaymentResponseDto>(
               await _uow.Payments.GetByIdAsync(id, ct) ?? throw new NotFoundException("Payment", id));

    public async Task<IEnumerable<PaymentResponseDto>> GetByOrderIdAsync(int orderId, CancellationToken ct = default)
    {
        if (await _uow.Orders.GetByIdAsync(orderId, ct) is null)
            throw new NotFoundException("Order", orderId);

        var list = await _uow.Payments.GetByOrderIdAsync(orderId);
        return _mapper.Map<IEnumerable<PaymentResponseDto>>(list);
    }

    public async Task<PaymentResponseDto> CreateAsync(PaymentCreateDto dto, CancellationToken ct = default)
    {
        if (await _uow.Orders.GetByIdAsync(dto.OrderId, ct) is null)
            throw new ValidationException($"Order {dto.OrderId} not exists.");

        var entity = _mapper.Map<Payment>(dto);
        entity.PaymentDate = DateTime.UtcNow;

        await _uow.Payments.AddAsync(entity, ct);
        await _uow.SaveAsync(ct);
        return _mapper.Map<PaymentResponseDto>(entity);
    }

    public async Task<PaymentResponseDto> UpdateAsync(int id, PaymentUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _uow.Payments.GetByIdAsync(id, ct)
                     ?? throw new NotFoundException("Payment", id);

        _mapper.Map(dto, entity);
        _uow.Payments.Update(entity);
        await _uow.SaveAsync(ct);
        return _mapper.Map<PaymentResponseDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _uow.Payments.GetByIdAsync(id, ct);
        if (entity == null) return false;

        _uow.Payments.Delete(entity);
        await _uow.SaveAsync(ct);
        return true;
    }
}
