using FluentValidation;
using ServiceCenterAppBLL.DTO.PaymentDto;

namespace ServiceCenterApp.Validations.Payment
{
    public class PaymentUpdateDtoValidator : AbstractValidator<PaymentUpdateDto>
    {
        public PaymentUpdateDtoValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.PaymentMethod).NotEmpty();
        }
    }
}
