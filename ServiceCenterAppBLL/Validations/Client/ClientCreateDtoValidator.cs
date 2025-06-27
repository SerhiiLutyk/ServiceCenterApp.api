using FluentValidation;
using ServiceCenterAppBLL.DTO.ClientDto;

namespace ServiceCenterApp.Validations.Client
{
    public class ClientCreateDtoValidator : AbstractValidator<ClientCreateDto>
    {
        public ClientCreateDtoValidator()
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
