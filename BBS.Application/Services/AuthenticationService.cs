using BBS.Application.Common;
using BBS.Application.DTOs.Users;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Services;

namespace BBS.Application.Services
{
    /// <summary>
    /// Provides authentication services, including user verification and JWT token generation.
    /// </summary>
    /// <remarks>Interacts with the user repository and JWT token service to handle login operations and
    /// credential validation.</remarks>
    public class AuthenticationService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtTokenService _jwtService;

        /// <summary>
        /// Initializes a new instance of the AuthenticationService class.
        /// </summary>
        /// <param name="userRepo">Repository used for accessing user data.</param>
        /// <param name="jwtService">Service for generating and validating JWT tokens.</param>
        public AuthenticationService(IUserRepository userRepo, IJwtTokenService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Logins a user by validating their credentials and generating JWT tokens upon successful authentication.
        /// </summary>
        /// <param name="request">LoginRequest request</param>
        /// <returns>LoginResponse</returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);

            if (user == null)
                throw new BusinessException("Invalid Credentials");

            var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isValid)
                throw new BusinessException("Invalid Credentials");

            var accessToken = _jwtService.GenerateAccessToken(user);

            var refreshToken = _jwtService.GenerateRefreshToken();

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}
