using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ServiceCenterApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Отримати інформацію про API
        /// </summary>
        [HttpGet]
        public ActionResult GetApiInfo()
        {
            var apiInfo = new
            {
                Name = "Service Center API",
                Version = "1.0.0",
                Description = "API для управління сервісним центром",
                Endpoints = new
                {
                    Clients = "/api/clients",
                    Orders = "/api/orders",
                    RepairTypes = "/api/repairtypes",
                    AdditionalServices = "/api/additionalservices",
                    Payments = "/api/payments",
                    Reports = "/api/reports"
                },
                Features = new[]
                {
                    "Управління клієнтами",
                    "Управління замовленнями",
                    "Управління типами ремонту",
                    "Управління додатковими послугами",
                    "Управління платежами",
                    "Звіти та статистика",
                    "Пагінація",
                    "Валідація даних",
                    "Логування",
                    "Кешування",
                    "Обробка винятків"
                }
            };

            return Ok(apiInfo);
        }

        /// <summary>
        /// Перевірити статус API
        /// </summary>
        [HttpGet("health")]
        public ActionResult GetHealthStatus()
        {
            var healthStatus = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0"
            };

            return Ok(healthStatus);
        }
    }
} 