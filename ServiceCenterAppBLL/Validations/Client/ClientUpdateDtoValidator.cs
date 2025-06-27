using FluentValidation;
using ServiceCenterAppBLL.DTO.ClientDto;

namespace ServiceCenterApp.Validations.Client
{
    public class ClientUpdateDtoValidator : AbstractValidator<ClientUpdateDto>
    {
        public ClientUpdateDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(2, 50);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .Matches(@"^\+?\d{10,15}$");
        }
    }
}
