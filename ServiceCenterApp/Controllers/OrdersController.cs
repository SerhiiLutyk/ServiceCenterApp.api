using Microsoft.AspNetCore.Mvc;
using ServiceCenterAppBLL.DTO.OrderDto;
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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Отримати всі замовлення з пагінацією
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<PagedList<OrderResponseDto>>> GetOrders(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortOrder = "asc")
        {
            var orders = await _orderService.GetAllAsync(page, pageSize, status, fromDate, toDate, sortBy, sortOrder);
            return Ok(orders);
        }

        /// <summary>
        /// Отримати замовлення за ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return Ok(order);
        }

        /// <summary>
        /// Створити нове замовлення
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            var createdOrder = await _orderService.CreateAsync(orderDto);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.OrderId }, createdOrder);
        }

        /// <summary>
        /// Оновити замовлення
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<OrderResponseDto>> UpdateOrder(int id, [FromBody] OrderUpdateDto orderDto)
        {
            var updatedOrder = await _orderService.UpdateAsync(id, orderDto);
            return Ok(updatedOrder);
        }

        /// <summary>
        /// Видалити замовлення
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Отримати платежі замовлення
        /// </summary>
        [HttpGet("{id}/payments")]
        public async Task<ActionResult> GetOrderPayments(int id)
        {
            var payments = await _orderService.GetOrderPaymentsAsync(id);
            return Ok(payments);
        }

        /// <summary>
        /// Змінити статус замовлення
        /// </summary>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderResponseDto>> UpdateOrderStatus(int id, [FromBody] string status)
        {
            var updatedOrder = await _orderService.UpdateStatusAsync(id, status);
            return Ok(updatedOrder);
        }

        /// <summary>
        /// Отримати замовлення за статусом
        /// </summary>
        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<PagedList<OrderResponseDto>>> GetOrdersByStatus(
            string status,
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            var orders = await _orderService.GetByStatusAsync(status, page, pageSize);
            return Ok(orders);
        }
    }
} 