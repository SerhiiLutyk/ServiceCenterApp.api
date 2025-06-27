using FluentValidation;
using ServiceCenterAppBLL.DTO.PaymentDto;

namespace ServiceCenterApp.Validations.Payment
{
    public class PaymentCreateDtoValidator : AbstractValidator<PaymentCreateDto>
    {
        public PaymentCreateDtoValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.PaymentMethod).NotEmpty();
        }
    }
}
