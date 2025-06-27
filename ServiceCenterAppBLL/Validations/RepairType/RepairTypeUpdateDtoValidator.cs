using FluentValidation;
using ServiceCenterAppBLL.DTO.RepairTypeDto;

namespace ServiceCenterApp.Validations.RepairType
{
    public class RepairTypeUpdateDtoValidator : AbstractValidator<RepairTypeUpdateDto>
    {
        public RepairTypeUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }
}
