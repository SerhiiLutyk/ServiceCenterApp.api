using FluentValidation;
using ServiceCenterAppBLL.DTO.ClientDto;

namespace ServiceCenterAppBLL.Validations.Client
{
    public class ClientUpdateDtoValidator : AbstractValidator<ClientUpdateDto>
    {
        public ClientUpdateDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ім'я обов'язкове")
                .Length(2, 50);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .Matches(@"^\+?\d{10,15}$").WithMessage("Телефон має бути у форматі +380XXXXXXXXX");
        }
    }
}
