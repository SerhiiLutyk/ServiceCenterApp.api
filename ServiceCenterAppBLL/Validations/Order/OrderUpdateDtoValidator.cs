using FluentValidation;
using ServiceCenterAppBLL.DTO.OrderDto;

namespace ServiceCenterApp.Validations.Order
{
    public class OrderUpdateDtoValidator : AbstractValidator<OrderUpdateDto>
    {
        public OrderUpdateDtoValidator()
        {
            RuleFor(x => x.Status).NotEmpty();
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }
}
