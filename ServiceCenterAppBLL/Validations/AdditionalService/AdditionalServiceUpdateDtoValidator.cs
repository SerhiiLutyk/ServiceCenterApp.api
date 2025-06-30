using FluentValidation;
using ServiceCenterAppBLL.DTO.AdditionalServiceDto;

namespace ServiceCenterAppBLL.Validations.AdditionalService
{
    public class AdditionalServiceUpdateDtoValidator : AbstractValidator<AdditionalServiceUpdateDto>
    {
        public AdditionalServiceUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва додаткової послуги обов'язкова")
                .Length(2, 100).WithMessage("Назва має бути від 2 до 100 символів");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Ціна не може бути від'ємною");
        }
    }
}
