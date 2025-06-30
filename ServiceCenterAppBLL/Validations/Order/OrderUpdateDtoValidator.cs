using FluentValidation;
using ServiceCenterAppBLL.DTO.OrderDto;

namespace ServiceCenterAppBLL.Validations.Order
{
    public class OrderUpdateDtoValidator : AbstractValidator<OrderUpdateDto>
    {
        public OrderUpdateDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Опис замовлення обов'язковий")
                .Length(10, 500).WithMessage("Опис має бути від 10 до 500 символів");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Статус замовлення обов'язковий")
                .MaximumLength(50).WithMessage("Статус не може перевищувати 50 символів");
        }
    }
}
