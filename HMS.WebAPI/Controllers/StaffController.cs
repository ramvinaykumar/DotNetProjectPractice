using HMS.Core.Dtos.Request.Staff;
using HMS.Core.Dtos.Response;
using HMS.Core.Interfaces;
using HMS.Core.Models.Staffs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _repo;
        public StaffController(IStaffRepository repo) => _repo = repo;

        /// <summary>Get all staff members.</summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAll([FromQuery] int? roleId, [FromQuery] bool? isActive)
        {
            var staff = await _repo.GetAllAsync(roleId, isActive);
            // Mask password hashes
            foreach (var s in staff) s.PasswordHash = null;
            return Ok(ApiResponse<IEnumerable<Staff>>.Ok(staff));
        }

        /// <summary>Get staff member by ID.</summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            var staff = await _repo.GetByIdAsync(id);
            if (staff is null) return NotFound(ApiResponse<object>.Fail("Staff not found."));
            staff.PasswordHash = null;
            return Ok(ApiResponse<Staff>.Ok(staff));
        }

        /// <summary>Create a new staff member.</summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] StaffCreateRequest request)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var id = await _repo.CreateAsync(request, hash);
            return CreatedAtAction(nameof(GetById), new { id },
                ApiResponse<object>.Ok(new { staffId = id }, "Staff created successfully."));
        }

        /// <summary>Update staff member details.</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] StaffUpdateRequest request)
        {
            var rows = await _repo.UpdateAsync(id, request);
            if (rows == 0) return NotFound(ApiResponse<object>.Fail("Staff not found."));
            return Ok(ApiResponse<object>.Ok(null, "Staff updated successfully."));
        }

        /// <summary>Get all roles.</summary>
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _repo.GetRolesAsync();
            return Ok(ApiResponse<IEnumerable<Role>>.Ok(roles));
        }
    }
}
