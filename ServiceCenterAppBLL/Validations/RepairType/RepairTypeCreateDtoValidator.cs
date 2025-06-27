using FluentValidation;
using ServiceCenterAppBLL.DTO.RepairTypeDto;

namespace ServiceCenterApp.Validations.RepairType
{
    public class RepairTypeCreateDtoValidator : AbstractValidator<RepairTypeCreateDto>
    {
        public RepairTypeCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }
}
