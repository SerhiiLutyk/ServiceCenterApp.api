using Microsoft.AspNetCore.Mvc;
using ServiceCenterAppBLL.DTO.RepairTypeDto;
using ServiceCenterAppBLL.Interfaces;
using ServiceCenterAppBLL.Filters;
using ServiceCenterAppBLL.Pagination;

namespace ServiceCenterApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LoggingFilter))]
    public class RepairTypesController : ControllerBase
    {
        private readonly IRepairTypeService _repairTypeService;

        public RepairTypesController(IRepairTypeService repairTypeService)
        {
            _repairTypeService = repairTypeService;
        }

        /// <summary>
        /// Отримати всі типи ремонту з пагінацією
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PagedList<RepairTypeResponseDto>>> GetRepairTypes(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null)
        {
            var repairTypes = await _repairTypeService.GetAllAsync(page, pageSize, searchTerm);
            return Ok(repairTypes);
        }

        /// <summary>
        /// Отримати тип ремонту за ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RepairTypeResponseDto>> GetRepairType(int id)
        {
            var repairType = await _repairTypeService.GetByIdAsync(id);
            return Ok(repairType);
        }

        /// <summary>
        /// Створити новий тип ремонту
        /// </summary>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<RepairTypeResponseDto>> CreateRepairType([FromBody] RepairTypeCreateDto repairTypeDto)
        {
            var createdRepairType = await _repairTypeService.CreateAsync(repairTypeDto);
            return CreatedAtAction(nameof(GetRepairType), new { id = createdRepairType.RepairTypeId }, createdRepairType);
        }

        /// <summary>
        /// Оновити тип ремонту
        /// </summary>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<RepairTypeResponseDto>> UpdateRepairType(int id, [FromBody] RepairTypeUpdateDto repairTypeDto)
        {
            var updatedRepairType = await _repairTypeService.UpdateAsync(id, repairTypeDto);
            return Ok(updatedRepairType);
        }

        /// <summary>
        /// Видалити тип ремонту
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRepairType(int id)
        {
            await _repairTypeService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Отримати замовлення для типу ремонту
        /// </summary>
        [HttpGet("{id}/orders")]
        public async Task<ActionResult> GetRepairTypeOrders(int id)
        {
            var orders = await _repairTypeService.GetRepairTypeOrdersAsync(id);
            return Ok(orders);
        }
    }
} 