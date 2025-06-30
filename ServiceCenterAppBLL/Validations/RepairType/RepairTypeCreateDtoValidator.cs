using FluentValidation;
using ServiceCenterAppBLL.DTO.RepairTypeDto;

namespace ServiceCenterAppBLL.Validations.RepairType
{
    public class RepairTypeCreateDtoValidator : AbstractValidator<RepairTypeCreateDto>
    {
        public RepairTypeCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва типу ремонту обов'язкова")
                .Length(2, 100).WithMessage("Назва має бути від 2 до 100 символів");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Ціна не може бути від'ємною");
        }
    }
}
