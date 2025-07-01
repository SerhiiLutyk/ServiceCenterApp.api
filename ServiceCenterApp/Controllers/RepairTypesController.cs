using Microsoft.AspNetCore.Mvc;
using ServiceCenterAppBLL.DTO.RepairTypeDto;
using ServiceCenterAppBLL.Interfaces;
using ServiceCenterAppBLL.Filters;
using ServiceCenterAppBLL.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace ServiceCenterApp.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<PagedList<RepairTypeResponseDto>>> GetRepairTypes(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "asc")
        {
            var repairTypes = await _repairTypeService.GetAllAsync(page, pageSize, searchTerm, sortBy, sortOrder);
            return Ok(repairTypes);
        }

        /// <summary>
        /// Отримати тип ремонту за ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<RepairTypeResponseDto>> GetRepairType(int id)
        {
            var repairType = await _repairTypeService.GetByIdAsync(id);
            return Ok(repairType);
        }

        /// <summary>
        /// Створити новий тип ремонту
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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