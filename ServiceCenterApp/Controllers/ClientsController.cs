using Microsoft.AspNetCore.Mvc;
using ServiceCenterAppBLL.DTO.ClientDto;
using ServiceCenterAppBLL.Interfaces;
using ServiceCenterAppBLL.Filters;
using ServiceCenterAppBLL.Pagination;

namespace ServiceCenterApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LoggingFilter))]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Отримати всіх клієнтів з пагінацією
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PagedList<ClientResponseDto>>> GetClients(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null)
        {
            var clients = await _clientService.GetAllAsync(page, pageSize, searchTerm);
            return Ok(clients);
        }

        /// <summary>
        /// Отримати клієнта за ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientResponseDto>> GetClient(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            return Ok(client);
        }

        /// <summary>
        /// Створити нового клієнта
        /// </summary>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<ClientResponseDto>> CreateClient([FromBody] ClientCreateDto clientDto)
        {
            var createdClient = await _clientService.CreateAsync(clientDto);
            return CreatedAtAction(nameof(GetClient), new { id = createdClient.ClientId }, createdClient);
        }

        /// <summary>
        /// Оновити клієнта
        /// </summary>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<ClientResponseDto>> UpdateClient(int id, [FromBody] ClientUpdateDto clientDto)
        {
            var updatedClient = await _clientService.UpdateAsync(id, clientDto);
            return Ok(updatedClient);
        }

        /// <summary>
        /// Видалити клієнта
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClient(int id)
        {
            await _clientService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Отримати замовлення клієнта
        /// </summary>
        [HttpGet("{id}/orders")]
        public async Task<ActionResult> GetClientOrders(int id)
        {
            var orders = await _clientService.GetClientOrdersAsync(id);
            return Ok(orders);
        }
    }
} 