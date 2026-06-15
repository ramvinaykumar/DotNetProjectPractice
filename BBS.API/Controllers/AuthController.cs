using BBS.Application.DTOs.Users;
using BBS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BBS.API.Controllers
{
    /// <summary>
    /// Provides API endpoints for user registration, authentication, token refresh, and profile retrieval.
    /// </summary>
    /// <remarks>Supports user account creation, login, token management, and access to authenticated user
    /// information.</remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Parameterized constructor for AuthController, accepting a user service to handle user-related operations.
        /// </summary>
        /// <param name="userService">IUserService userService</param>
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="request">The registration information for the new user.</param>
        /// <returns>An IActionResult containing the user ID if registration is successful.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var userId = await _userService.RegisterAsync(request);

            return Success(new { UserId = userId }, "User registered successfully!");
        }

        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <returns>An IActionResult containing the authentication result.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _userService.LoginAsync(request);

            return Success(result, "User logged in successfully!");
        }

        /// <summary>
        /// Refreshes the access token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">string refreshToken</param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var result = await _userService.RefreshTokenAsync(refreshToken);

            return Success(result, "Token refreshed successfully!");
        }

        /// <summary>
        /// Get user profile information based on the authenticated user's claims.
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return Success(new { Email = email }, "profile information based on the authenticated user's claims fetched successfully!");
        }
    }
}
