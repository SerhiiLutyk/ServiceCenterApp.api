using FluentValidation;
using ServiceCenterAppBLL.DTO.AdditionalServiceDto;

namespace ServiceCenterApp.Validations.AdditionalService
{
    public class AdditionalServiceUpdateDtoValidator : AbstractValidator<AdditionalServiceUpdateDto>
    {
        public AdditionalServiceUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }
}
