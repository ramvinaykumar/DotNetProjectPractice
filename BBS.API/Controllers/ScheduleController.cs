using BBS.Application.DTOs.Schedule;
using BBS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBS.API.Controllers
{
    /// <summary>
    /// Handles schedule management operations including retrieval, creation, updating, and deletion of schedules via
    /// API endpoints.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : BaseController
    {
        private readonly IScheduleService _service;

        /// <summary>
        /// Initializes a new instance of the ScheduleController class.
        /// </summary>
        /// <param name="service">Service used to manage schedule operations.</param>
        public ScheduleController(IScheduleService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all schedules asynchronously.
        /// </summary>
        /// <returns>An IActionResult containing the list of schedules and a success message.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var schedules = await _service.GetAllAsync();
            return Success(schedules, "Successfully fetched all schedules.");
        }

        /// <summary>
        /// Retrieves a schedule by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the schedule to retrieve.</param>
        /// <returns>An IActionResult containing the retrieved schedule or an error response.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _service.GetByIdAsync(id);
            return Success(schedule, $"Successfully fetched schedule with ID {id}.");
        }

        /// <summary>
        /// Creates a new schedule using the specified request data.
        /// </summary>
        /// <param name="request">The details required to create a new schedule.</param>
        /// <returns>An IActionResult containing the result of the create operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateScheduleRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Success(result, "Schedule created successfully.");
        }

        /// <summary>
        /// Updates an existing schedule identified by its unique identifier using the specified request data.
        /// </summary>
        /// <param name="id">int id</param>
        /// <param name="request">UpdateScheduleRequest request</param>
        /// <returns>An IActionResult containing the result of the update operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateScheduleRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return Success(result, "Schedule updated successfully.");
        }

        /// <summary>
        /// Deletes a schedule identified by its unique identifier.
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns>An IActionResult containing the retrieved schedule or an error response.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Success("Schedule deleted successfully.");
        }
    }
}
