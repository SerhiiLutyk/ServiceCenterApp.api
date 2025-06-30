using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ServiceCenterAppBLL.Filters
{
    public class CacheFilter : IActionFilter
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheFilter> _logger;

        public CacheFilter(IMemoryCache cache, ILogger<CacheFilter> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Генеруємо ключ кешу на основі URL та параметрів
            var cacheKey = GenerateCacheKey(context);
            
            if (_cache.TryGetValue(cacheKey, out var cachedResult))
            {
                context.Result = new OkObjectResult(cachedResult);
                _logger.LogInformation("Повернуто результат з кешу для ключа: {CacheKey}", cacheKey);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is OkObjectResult okResult)
            {
                var cacheKey = GenerateCacheKey(context);
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _cache.Set(cacheKey, okResult.Value, cacheOptions);
                _logger.LogInformation("Збережено результат у кеш з ключем: {CacheKey}", cacheKey);
            }
        }

        private string GenerateCacheKey(ActionExecutingContext context)
        {
            var path = context.HttpContext.Request.Path.Value;
            var queryString = context.HttpContext.Request.QueryString.Value;
            return $"{path}{queryString}";
        }

        private string GenerateCacheKey(ActionExecutedContext context)
        {
            var path = context.HttpContext.Request.Path.Value;
            var queryString = context.HttpContext.Request.QueryString.Value;
            return $"{path}{queryString}";
        }
    }
} 