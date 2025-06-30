using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ServiceCenterAppBLL.Filters
{
    public class LoggingFilter : IActionFilter
    {
        private readonly ILogger<LoggingFilter> _logger;
        private Stopwatch _stopwatch;

        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();
            var actionName = context.ActionDescriptor.DisplayName;
            var httpMethod = context.HttpContext.Request.Method;
            var path = context.HttpContext.Request.Path;

            _logger.LogInformation("Початок виконання {HttpMethod} {Path} - {ActionName}", 
                httpMethod, path, actionName);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch?.Stop();
            var actionName = context.ActionDescriptor.DisplayName;
            var httpMethod = context.HttpContext.Request.Method;
            var path = context.HttpContext.Request.Path;
            var statusCode = context.HttpContext.Response.StatusCode;

            _logger.LogInformation("Завершено {HttpMethod} {Path} - {ActionName} за {ElapsedMs}ms з кодом {StatusCode}", 
                httpMethod, path, actionName, _stopwatch?.ElapsedMilliseconds, statusCode);
        }
    }
} 