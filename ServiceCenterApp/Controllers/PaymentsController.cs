using Microsoft.AspNetCore.Mvc;
using ServiceCenterAppBLL.DTO.PaymentDto;
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
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Отримати всі платежі з пагінацією
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<PagedList<PaymentResponseDto>>> GetPayments(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string? paymentMethod = null)
        {
            var payments = await _paymentService.GetAllAsync(page, pageSize, fromDate, toDate, paymentMethod);
            return Ok(payments);
        }

        /// <summary>
        /// Отримати платіж за ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<PaymentResponseDto>> GetPayment(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            return Ok(payment);
        }

        /// <summary>
        /// Створити новий платіж
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<PaymentResponseDto>> CreatePayment([FromBody] PaymentCreateDto paymentDto)
        {
            var createdPayment = await _paymentService.CreateAsync(paymentDto);
            return CreatedAtAction(nameof(GetPayment), new { id = createdPayment.PaymentId }, createdPayment);
        }

        /// <summary>
        /// Оновити платіж
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<PaymentResponseDto>> UpdatePayment(int id, [FromBody] PaymentUpdateDto paymentDto)
        {
            var updatedPayment = await _paymentService.UpdateAsync(id, paymentDto);
            return Ok(updatedPayment);
        }

        /// <summary>
        /// Видалити платіж
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePayment(int id)
        {
            await _paymentService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Отримати платежі за замовленням
        /// </summary>
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult> GetPaymentsByOrder(int orderId)
        {
            var payments = await _paymentService.GetPaymentsByOrderAsync(orderId);
            return Ok(payments);
        }

        /// <summary>
        /// Отримати статистику платежів
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult> GetPaymentStatistics(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var statistics = await _paymentService.GetPaymentStatisticsAsync(fromDate, toDate);
            return Ok(statistics);
        }
    }
} 