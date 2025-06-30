using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ServiceCenterAppBLL.Exceptions;
using System.Net;

namespace ServiceCenterAppBLL.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Необроблений виняток: {Message}", context.Exception.Message);

            var result = context.Exception switch
            {
                NotFoundException => new ObjectResult(new { error = context.Exception.Message })
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                },
                BadRequestException => new ObjectResult(new { error = context.Exception.Message })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                },
                ValidationException => new ObjectResult(new { error = context.Exception.Message })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                },
                UnauthorizedException => new ObjectResult(new { error = context.Exception.Message })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                },
                AccessDeniedException => new ObjectResult(new { error = context.Exception.Message })
                {
                    StatusCode = (int)HttpStatusCode.Forbidden
                },
                ConflictException => new ObjectResult(new { error = context.Exception.Message })
                {
                    StatusCode = (int)HttpStatusCode.Conflict
                },
                BusinessException => new ObjectResult(new { error = context.Exception.Message })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                },
                _ => new ObjectResult(new { error = "Внутрішня помилка сервера" })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                }
            };

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
} 