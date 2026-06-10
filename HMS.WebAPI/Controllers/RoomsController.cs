using HMS.Core.Dtos.Request.Rooms;
using HMS.Core.Dtos.Response;
using HMS.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepository _repo;
        public RoomsController(IRoomRepository repo) => _repo = repo;

        /// <summary>Get all rooms with optional filters.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? status,
            [FromQuery] int? roomTypeId,
            [FromQuery] int? floor)
        {
            var rooms = await _repo.GetAllAsync(status, roomTypeId, floor);
            return Ok(ApiResponse<IEnumerable<Core.Models.Room>>.Ok(rooms));
        }

        /// <summary>Get room by ID.</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _repo.GetByIdAsync(id);
            if (room is null) return NotFound(ApiResponse<object>.Fail("Room not found."));
            return Ok(ApiResponse<Core.Models.Room>.Ok(room));
        }

        /// <summary>Search available rooms for given dates.</summary>
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailable([FromQuery] RoomAvailabilityRequest request)
        {
            if (request.CheckOutDate <= request.CheckInDate)
                return BadRequest(ApiResponse<object>.Fail("Check-out must be after check-in."));
            var rooms = await _repo.GetAvailableAsync(request);
            return Ok(ApiResponse<IEnumerable<Core.Models.Room>>.Ok(rooms));
        }

        /// <summary>Create a new room.</summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([FromBody] RoomCreateRequest request)
        {
            var id = await _repo.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id },
                ApiResponse<object>.Ok(new { roomId = id }, "Room created successfully."));
        }

        /// <summary>Update room details.</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] RoomUpdateRequest request)
        {
            var rows = await _repo.UpdateAsync(id, request);
            if (rows == 0) return NotFound(ApiResponse<object>.Fail("Room not found."));
            return Ok(ApiResponse<object>.Ok(null, "Room updated successfully."));
        }

        /// <summary>Update room status only (e.g. Available, Maintenance, Cleaning).</summary>
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] RoomStatusUpdateRequest request)
        {
            var rows = await _repo.UpdateStatusAsync(id, request);
            if (rows == 0) return NotFound(ApiResponse<object>.Fail("Room not found."));
            return Ok(ApiResponse<object>.Ok(null, "Room status updated."));
        }

        // ─── Room Types ───────────────────────────────────────────────────────────

        /// <summary>Get all room types.</summary>
        [HttpGet("types")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoomTypes([FromQuery] bool isActive = true)
        {
            var types = await _repo.GetRoomTypesAsync(isActive);
            return Ok(ApiResponse<IEnumerable<Core.Models.RoomType>>.Ok(types));
        }

        /// <summary>Create or update a room type.</summary>
        [HttpPost("types")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpsertRoomType([FromBody] RoomTypeUpsertRequest request)
        {
            var id = await _repo.UpsertRoomTypeAsync(request);
            return Ok(ApiResponse<object>.Ok(new { roomTypeId = id }, "Room type saved successfully."));
        }
    }
}
