using BBS.Application.DTOs.Booking;
using BBS.Application.Interfaces.Services;
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
        public async Task<IActionResult> Create(CreateBookingRequest request)
        {
            var result = await _service.CreateAsync(request);

            return Success(result, "Booking created successfully.");
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, UpdateBookingRequest request)
        //{
        //    await _service.UpdateBookingAsync(id, request);

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _service.DeleteBookingAsync(id);

        //    return NoContent();
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var data = await _service.GetBookingByIdAsync(id);
        //    return Ok(data);
        //}
    }
}
