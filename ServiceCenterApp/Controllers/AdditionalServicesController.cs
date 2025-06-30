using Microsoft.AspNetCore.Mvc;
using ServiceCenterAppBLL.DTO.AdditionalServiceDto;
using ServiceCenterAppBLL.Interfaces;
using ServiceCenterAppBLL.Filters;
using ServiceCenterAppBLL.Pagination;

namespace ServiceCenterApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LoggingFilter))]
    public class AdditionalServicesController : ControllerBase
    {
        private readonly IAdditionalServiceService _additionalServiceService;

        public AdditionalServicesController(IAdditionalServiceService additionalServiceService)
        {
            _additionalServiceService = additionalServiceService;
        }

        /// <summary>
        /// Отримати всі додаткові послуги з пагінацією
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PagedList<AdditionalServiceResponseDto>>> GetAdditionalServices(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null)
        {
            var services = await _additionalServiceService.GetAllAsync(page, pageSize, searchTerm);
            return Ok(services);
        }

        /// <summary>
        /// Отримати додаткову послугу за ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AdditionalServiceResponseDto>> GetAdditionalService(int id)
        {
            var service = await _additionalServiceService.GetByIdAsync(id);
            return Ok(service);
        }

        /// <summary>
        /// Створити нову додаткову послугу
        /// </summary>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<AdditionalServiceResponseDto>> CreateAdditionalService([FromBody] AdditionalServiceCreateDto serviceDto)
        {
            var createdService = await _additionalServiceService.CreateAsync(serviceDto);
            return CreatedAtAction(nameof(GetAdditionalService), new { id = createdService.ServiceId }, createdService);
        }

        /// <summary>
        /// Оновити додаткову послугу
        /// </summary>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<AdditionalServiceResponseDto>> UpdateAdditionalService(int id, [FromBody] AdditionalServiceUpdateDto serviceDto)
        {
            var updatedService = await _additionalServiceService.UpdateAsync(id, serviceDto);
            return Ok(updatedService);
        }

        /// <summary>
        /// Видалити додаткову послугу
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdditionalService(int id)
        {
            await _additionalServiceService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Отримати замовлення для додаткової послуги
        /// </summary>
        [HttpGet("{id}/orders")]
        public async Task<ActionResult> GetAdditionalServiceOrders(int id)
        {
            var orders = await _additionalServiceService.GetAdditionalServiceOrdersAsync(id);
            return Ok(orders);
        }
    }
} 