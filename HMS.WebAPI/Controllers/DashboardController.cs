using HMS.Core.Dtos.Response;
using HMS.Core.Interfaces;
using HMS.Core.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _repo;
        public DashboardController(IDashboardRepository repo) => _repo = repo;

        /// <summary>Get full dashboard summary (stats, recent bookings, payments, room status).</summary>
        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            var (summary, bookings, payments, roomStatus) = await _repo.GetSummaryAsync();
            var response = new DashboardResponse
            {
                Summary = summary,
                RecentBookings = bookings,
                RecentPayments = payments,
                RoomStatusBreakdown = roomStatus
            };
            return Ok(ApiResponse<DashboardResponse>.Ok(response));
        }

        /// <summary>Revenue chart data by month for a given year.</summary>
        [HttpGet("revenue-chart")]
        public async Task<IActionResult> GetRevenueChart([FromQuery] int? year)
        {
            var data = await _repo.GetRevenueChartAsync(year);
            return Ok(ApiResponse<IEnumerable<MonthlyRevenue>>.Ok(data));
        }

        /// <summary>Occupancy rate trend for the last 30 days.</summary>
        [HttpGet("occupancy-chart")]
        public async Task<IActionResult> GetOccupancyChart()
        {
            var data = await _repo.GetOccupancyChartAsync();
            return Ok(ApiResponse<IEnumerable<OccupancyData>>.Ok(data));
        }
    }
}
