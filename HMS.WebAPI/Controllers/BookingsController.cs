using HMS.Core.Dtos.Request.Bookings;
using HMS.Core.Dtos.Response;
using HMS.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository _repo;
        public BookingsController(IBookingRepository repo) => _repo = repo;

        /// <summary>Get paginated bookings with filters.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BookingQueryRequest query)
        {
            var (bookings, total) = await _repo.GetAllAsync(query);
            return Ok(new PagedResponse<Core.Models.Booking>
            {
                Data = bookings,
                TotalCount = total,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            });
        }

        /// <summary>Get booking by ID including associated payments.</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var (booking, payments) = await _repo.GetByIdWithPaymentsAsync(id);
            if (booking is null) return NotFound(ApiResponse<object>.Fail("Booking not found."));
            return Ok(ApiResponse<object>.Ok(new { booking, payments }));
        }

        /// <summary>Create a new booking.</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingCreateRequest request)
        {
            var id = await _repo.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id },
                ApiResponse<object>.Ok(new { bookingId = id }, "Booking created successfully."));
        }

        /// <summary>Update booking dates and details.</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookingUpdateRequest request)
        {
            var rows = await _repo.UpdateAsync(id, request);
            if (rows == 0) return NotFound(ApiResponse<object>.Fail("Booking not found or cannot be updated."));
            return Ok(ApiResponse<object>.Ok(null, "Booking updated successfully."));
        }

        /// <summary>Check in a guest.</summary>
        [HttpPost("{id:int}/checkin")]
        public async Task<IActionResult> CheckIn(int id, [FromBody] BookingCheckInRequest request)
        {
            await _repo.CheckInAsync(id, request.StaffId);
            return Ok(ApiResponse<object>.Ok(null, "Guest checked in successfully."));
        }

        /// <summary>Check out a guest.</summary>
        [HttpPost("{id:int}/checkout")]
        public async Task<IActionResult> CheckOut(int id)
        {
            await _repo.CheckOutAsync(id);
            return Ok(ApiResponse<object>.Ok(null, "Guest checked out successfully."));
        }

        /// <summary>Cancel a booking.</summary>
        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id, [FromBody] BookingCancelRequest request)
        {
            await _repo.CancelAsync(id, request.CancellationReason);
            return Ok(ApiResponse<object>.Ok(null, "Booking cancelled successfully."));
        }
    }
}
