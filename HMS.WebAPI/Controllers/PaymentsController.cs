using HMS.Core.Dtos.Request.Payment;
using HMS.Core.Dtos.Response;
using HMS.Core.Interfaces;
using HMS.Core.Models.Dashboard;
using HMS.Core.Models.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _repo;
        public PaymentsController(IPaymentRepository repo) => _repo = repo;

        /// <summary>Get paginated payment records.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaymentQueryRequest query)
        {
            var (payments, total) = await _repo.GetAllAsync(query);
            return Ok(new PagedResponse<Payment>
            {
                Data = payments,
                TotalCount = total,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            });
        }

        /// <summary>Record a new payment.</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentCreateRequest request)
        {
            var id = await _repo.CreateAsync(request);
            return Ok(ApiResponse<object>.Ok(new { paymentId = id }, "Payment recorded successfully."));
        }

        /// <summary>Refund a payment.</summary>
        [HttpPost("{id:int}/refund")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Refund(int id, [FromBody] string? notes)
        {
            var rows = await _repo.RefundAsync(id, notes);
            if (rows == 0) return NotFound(ApiResponse<object>.Fail("Payment not found or already refunded."));
            return Ok(ApiResponse<object>.Ok(null, "Payment refunded successfully."));
        }

        /// <summary>Get all available payment methods.</summary>
        [HttpGet("methods")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMethods()
        {
            var methods = await _repo.GetMethodsAsync();
            return Ok(ApiResponse<IEnumerable<PaymentMethod>>.Ok(methods));
        }

        /// <summary>Get monthly revenue summary for a year.</summary>
        [HttpGet("revenue/monthly")]
        public async Task<IActionResult> GetMonthlyRevenue([FromQuery] int? year)
        {
            var data = await _repo.GetMonthlyRevenueAsync(year);
            return Ok(ApiResponse<IEnumerable<MonthlyRevenue>>.Ok(data));
        }
    }
}
