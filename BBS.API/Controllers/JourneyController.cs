using BBS.Application.Common;
using BBS.Application.DTOs.Journey;
using BBS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBS.API.Controllers
{
    /// <summary>
    /// API controller for managing journey-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JourneyController : BaseController
    {
        private readonly IJourneyService _journeyService;

        public JourneyController(IJourneyService journeyService)
        {
            _journeyService = journeyService;
        }

        /// <summary>
        /// Searches available journeys based on source,
        /// destination and travel date.
        /// </summary>
        [HttpPost("search")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<QueryResult<SearchJourneyResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchJourneys([FromBody] SearchJourneyRequest request)
        {
            var result = await _journeyService.SearchJourneysAsync(request);

            return Success(result, result.HasData ? "Journeys retrieved successfully." : "No journeys found for the selected criteria.");
        }
    }
}
