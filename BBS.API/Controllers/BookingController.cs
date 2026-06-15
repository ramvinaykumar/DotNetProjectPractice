using BBS.Application.DTOs.Booking;
using BBS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BBS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetAllBookingsAsync();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingRequest request)
        {
            var id = await _service
                    .CreateBookingAsync(request);

            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBookingRequest request)
        {
            await _service.UpdateBookingAsync(id, request);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteBookingAsync(id);

            return NoContent();
        }
    }
}
