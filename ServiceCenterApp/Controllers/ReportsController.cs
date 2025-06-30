using Microsoft.AspNetCore.Mvc;
using ServiceCenterAppBLL.Interfaces;
using ServiceCenterAppBLL.Filters;
using Microsoft.AspNetCore.Authorization;

namespace ServiceCenterApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LoggingFilter))]
    public class ReportsController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IClientService _clientService;

        public ReportsController(
            IOrderService orderService,
            IPaymentService paymentService,
            IClientService clientService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _clientService = clientService;
        }

        /// <summary>
        /// Отримати загальну статистику сервісного центру
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<ActionResult> GetDashboardStatistics(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var statistics = new
            {
                TotalOrders = await _orderService.GetTotalOrdersCountAsync(fromDate, toDate),
                CompletedOrders = await _orderService.GetCompletedOrdersCountAsync(fromDate, toDate),
                PendingOrders = await _orderService.GetPendingOrdersCountAsync(fromDate, toDate),
                TotalRevenue = await _paymentService.GetTotalRevenueAsync(fromDate, toDate),
                TotalClients = await _clientService.GetTotalClientsCountAsync(fromDate, toDate),
                AverageOrderValue = await _orderService.GetAverageOrderValueAsync(fromDate, toDate)
            };

            return Ok(statistics);
        }

        /// <summary>
        /// Отримати звіт по замовленнях за період
        /// </summary>
        [HttpGet("orders")]
        public async Task<ActionResult> GetOrdersReport(
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate,
            [FromQuery] string? status = null)
        {
            var report = await _orderService.GetOrdersReportAsync(fromDate, toDate, status);
            return Ok(report);
        }

        /// <summary>
        /// Отримати звіт по доходах за період
        /// </summary>
        [HttpGet("revenue")]
        public async Task<ActionResult> GetRevenueReport(
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate,
            [FromQuery] string? groupBy = "day")
        {
            var report = await _paymentService.GetRevenueReportAsync(fromDate, toDate, groupBy);
            return Ok(report);
        }

        /// <summary>
        /// Отримати звіт по популярних типах ремонту
        /// </summary>
        [HttpGet("popular-repair-types")]
        public async Task<ActionResult> GetPopularRepairTypesReport(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int top = 10)
        {
            var report = await _orderService.GetPopularRepairTypesReportAsync(fromDate, toDate, top);
            return Ok(report);
        }

        /// <summary>
        /// Отримати звіт по клієнтах
        /// </summary>
        [HttpGet("clients")]
        public async Task<ActionResult> GetClientsReport(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var report = await _clientService.GetClientsReportAsync(fromDate, toDate);
            return Ok(report);
        }

        /// <summary>
        /// Отримати звіт по методах оплати
        /// </summary>
        [HttpGet("payment-methods")]
        public async Task<ActionResult> GetPaymentMethodsReport(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var report = await _paymentService.GetPaymentMethodsReportAsync(fromDate, toDate);
            return Ok(report);
        }
    }
} 