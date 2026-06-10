using HMS.Core.Dtos.Request.Auth;
using HMS.Core.Dtos.Response;
using HMS.Core.Interfaces;
using HMS.Core.Models.Staffs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IStaffRepository _staff;
        private readonly ITokenService _token;

        public AuthController(IStaffRepository staff, ITokenService token)
        {
            _staff = staff;
            _token = token;
        }

        /// <summary>Login with email and password. Returns JWT token.</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var staff = await _staff.GetByEmailAsync(request.Email);
            if (staff is null || !BCrypt.Net.BCrypt.Verify(request.Password, staff.PasswordHash))
                return Unauthorized(ApiResponse<object>.Fail("Invalid email or password."));

            if (!staff.IsActive)
                return Unauthorized(ApiResponse<object>.Fail("Account is deactivated."));

            var token = _token.GenerateToken(staff);
            var expiry = _token.GetExpiry();
            staff.PasswordHash = null; // never return hash

            var response = new LoginResponse
            {
                Token = token,
                ExpiresAt = expiry,
                Staff = new StaffResponse
                {
                    StaffId = staff.StaffId,
                    RoleId = staff.RoleId,
                    RoleName = staff.RoleName,
                    FirstName = staff.FirstName,
                    LastName = staff.LastName,
                    Email = staff.Email,
                    PhoneNumber = staff.PhoneNumber,
                    Salary = staff.Salary,
                    HireDate = staff.HireDate,
                    IsActive = staff.IsActive,
                    ProfileImage = staff.ProfileImage,
                    CreatedAt = staff.CreatedAt,
                }
            };

            return Ok(ApiResponse<LoginResponse>.Ok(response, "Login successful."));
        }

        /// <summary>Change current user's password.</summary>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var staffIdClaim = User.FindFirst("staffId")?.Value;
            if (staffIdClaim is null) return Unauthorized();

            var staffId = int.Parse(staffIdClaim);
            var staff = await _staff.GetByIdAsync(staffId);
            if (staff is null) return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, staff.PasswordHash))
                return BadRequest(ApiResponse<object>.Fail("Current password is incorrect."));

            var newHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _staff.UpdatePasswordAsync(staffId, newHash);
            return Ok(ApiResponse<object>.Ok(null, "Password changed successfully."));
        }

        /// <summary>Return current authenticated staff profile.</summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var staffIdClaim = User.FindFirst("staffId")?.Value;
            if (staffIdClaim is null) return Unauthorized();

            var staff = await _staff.GetByIdAsync(int.Parse(staffIdClaim));
            if (staff is null) return NotFound();
            staff.PasswordHash = null;
            return Ok(ApiResponse<Staff>.Ok(staff));
        }
    }
}
