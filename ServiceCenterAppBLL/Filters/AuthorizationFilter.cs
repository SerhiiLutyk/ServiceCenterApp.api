using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceCenterAppBLL.Exceptions;

namespace ServiceCenterAppBLL.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Тут можна додати логіку авторизації
            // Наприклад, перевірка JWT токенів, ролей користувачів тощо
            
            // Приклад перевірки заголовка авторизації
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            
            if (string.IsNullOrEmpty(authHeader))
            {
                context.Result = new UnauthorizedObjectResult(new { error = "Необхідна авторизація" });
                return;
            }

            // Додаткова логіка перевірки токена може бути додана тут
            if (!authHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedObjectResult(new { error = "Неправильний формат токена" });
                return;
            }
        }
    }
} 