using BBS.Application.Common;
using BBS.Application.DTOs.Reports;
using BBS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBS.API.Controllers
{
    /// <summary>
    /// Handles HTTP requests for generating and retrieving reports.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : BaseController
    {
        private readonly IRouteSeatAvailabilityService _reportingService;

        /// <summary>
        /// Parameterized constructor for ReportsController.
        /// </summary>
        /// <param name="reportingService">IReportingService reportingService</param>
        public ReportsController(IRouteSeatAvailabilityService reportingService)
        {
            _reportingService = reportingService;
        }

        /// <summary>
        /// Returns route-wise seat availability.
        /// </summary>
        /// <param name="routeId">Route Id.</param>
        /// <returns>Seat availability report.</returns>
        [HttpGet("route-seat-availability")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<RouteSeatAvailabilityResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetRouteSeatAvailability([FromQuery] int routeId)
        {
            var result = await _reportingService.GetRouteSeatAvailabilityAsync(routeId);

            return Success(result, "Seat availability retrieved successfully.");
        }
    }
}
