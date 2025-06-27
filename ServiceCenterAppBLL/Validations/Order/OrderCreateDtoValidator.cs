using FluentValidation;
using ServiceCenterAppBLL.DTO.OrderDto;

namespace ServiceCenterApp.Validations.Order
{
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
    {
        public OrderCreateDtoValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.RepairTypeId).NotEmpty();
            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}
