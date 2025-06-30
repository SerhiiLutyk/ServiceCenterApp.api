using FluentValidation;
using ServiceCenterAppBLL.DTO.OrderDto;

namespace ServiceCenterAppBLL.Validations.Order
{
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
    {
        public OrderCreateDtoValidator()
        {
            RuleFor(x => x.ClientId)
                .GreaterThan(0).WithMessage("ID клієнта має бути більше 0");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Опис замовлення обов'язковий")
                .Length(10, 500).WithMessage("Опис має бути від 10 до 500 символів");

            RuleFor(x => x.RepairTypeId)
                .GreaterThan(0).WithMessage("ID типу ремонту має бути більше 0");
        }
    }
}
