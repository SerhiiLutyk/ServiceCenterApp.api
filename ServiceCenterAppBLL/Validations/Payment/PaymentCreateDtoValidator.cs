using FluentValidation;
using ServiceCenterAppBLL.DTO.PaymentDto;

namespace ServiceCenterAppBLL.Validations.Payment
{
    public class PaymentCreateDtoValidator : AbstractValidator<PaymentCreateDto>
    {
        public PaymentCreateDtoValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("ID замовлення має бути більше 0");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Сума платежу має бути більше 0");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Метод оплати обов'язковий")
                .MaximumLength(50).WithMessage("Метод оплати не може перевищувати 50 символів");
        }
    }
}
