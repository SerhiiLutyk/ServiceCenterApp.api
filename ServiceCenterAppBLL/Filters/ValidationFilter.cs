using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;
using ServiceCenterAppBLL.Exceptions;

namespace ServiceCenterAppBLL.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                var errorMessage = string.Join("; ", errors);
                throw new ServiceCenterAppBLL.Exceptions.ValidationException(errorMessage);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Логіка після виконання дії
        }
    }
} 