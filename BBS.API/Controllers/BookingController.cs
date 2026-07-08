using BBS.Application.DTOs.Booking;
using BBS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBS.API.Controllers
{
    /// <summary>
    /// Handles HTTP requests related to booking operations, including retrieval and creation of bookings.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseController
    {
        private readonly IBookingService _service;

        /// <summary>
        /// Initializes a new instance of the BookingController class.
        /// </summary>
        /// <param name="service">The service used to manage booking operations.</param>
        public BookingController(IBookingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all booking records.
        /// </summary>
        /// <returns>An IActionResult containing the collection of bookings and a success message.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetAllAsync();

            return Success(data, "Booking data fetched successfully!");
        }

        /// <summary>
        /// Creates a new booking based on the provided request data, and returns the created booking along with a success message.
        /// </summary>
        /// <param name="request">CreateBookingRequest request</param>
        /// <returns>An IActionResult containing the newly added of bookings and a success message.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateBookingRequest request)
        {
            var result = await _service.CreateAsync(request);

            return Success(result, "Booking created successfully.");
        }

        /// <summary>
        /// Retrieves booking information by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the booking.</param>
        /// <returns>An IActionResult containing the booking data and a success message.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            return Success(data, "Booking data fetched successfully!");
        }

        /// <summary>
        /// Updates an existing booking with the specified information.
        /// </summary>
        /// <param name="id">The unique identifier of the booking to update.</param>
        /// <param name="request">The updated booking details.</param>
        /// <returns>An IActionResult containing the result of the update operation.</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateBookingRequest request)
        {
            var result = await _service.UpdateBookingAsync(id, request);

            return Success(result, "Booking updated successfully.");
        }

        /// <summary>
        /// Cancels the booking with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the booking to cancel.</param>
        /// <returns>An IActionResult indicating the outcome of the cancellation.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Cancel(int id)
        {
            await _service.CancelAsync(id);

            return Success("Booking cancelled successfully.");
        }
    }
}
