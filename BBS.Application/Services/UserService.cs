using BBS.Application.Common;
using BBS.Application.DTOs.Users;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Services;
using BBS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BBS.Application.Services
{
    /// <summary>
    /// Provides user registration, authentication, and token management services.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IJwtTokenService _jwtService;

        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// Parameterized constructor for UserService.
        /// </summary>
        /// <param name="userRepository">IUserRepository userRepository</param>
        /// <param name="jwtService">IJwtTokenService jwtService</param>
        /// <param name="logger">ILogger logger</param>
        public UserService(IUserRepository userRepository, IJwtTokenService jwtService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user with the specified details asynchronously.
        /// </summary>
        /// <param name="request">The registration request containing user information such as email, username, password, and role.</param>
        /// <returns>A task that represents the asynchronous operation, containing the response with the registered user's data.</returns>
        /// <exception cref="BusinessException">Thrown when a user with the provided email already exists.</exception>
        public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request)
        {
            _logger.LogInformation("Registering user with email: {Email} and username: {UserName}", request.Email, request.UserName);

            var exists = await _userRepository.UserExistsAsync(request.Email);

            if (exists)
            {
                throw new BusinessException("Email already exists");
            }

            _logger.LogInformation("Creating user entity for email: {Email}", request.Email);

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role
            };

            var userId = await _userRepository.CreateUserAsync(user);

            _logger.LogInformation("User created with ID: {UserId} for email: {Email}", userId, request.Email);

            return await GetDataById(userId);
        }

        /// <summary>
        /// Authenticates a user with the provided credentials and issues access and refresh tokens.
        /// </summary>
        /// <param name="request">The login request containing the user's email and password.</param>
        /// <returns>A LoginResponse containing the access token, refresh token, and token expiry date.</returns>
        /// <exception cref="BusinessException">Thrown when the email or password is invalid.</exception>
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            _logger.LogInformation("Attempting login for email: {Email}, password: {Password}", request.Email, request.Password);

            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new BusinessException("Invalid credentials");
            }
            _logger.LogInformation("User found for email: {Email}, verifying password", request.Email);
            bool passwordMatched = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!passwordMatched)
            {
                throw new BusinessException("Invalid credentials");
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            _logger.LogInformation("Access token generated for user ID: {UserId} with token: {accessToken}", user.UserId, accessToken);

            var refreshToken = _jwtService.GenerateRefreshToken();
            _logger.LogInformation("Refresh token generated for user ID: {UserId} with token: {refreshToken}", user.UserId, refreshToken);

            var hashedRefreshToken = BCrypt.Net.BCrypt.HashPassword(refreshToken);

            _logger.LogInformation("Saving hashed refresh token for user ID: {UserId} with refreshtoken: {hashedRefreshToken}", user.UserId, hashedRefreshToken);

            await _userRepository.SaveRefreshTokenAsync(
                    new RefreshToken
                    {
                        UserId = user.UserId,
                        TokenHash = hashedRefreshToken,
                        ExpiryDate = DateTime.UtcNow.AddDays(7),
                        IsRevoked = false
                    });

            _logger.LogInformation("Login successful for user ID: {UserId} with accessToken: {accessToken}, refreshtoken: {refreshToken}", user.UserId, accessToken, refreshToken);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddHours(1)
            };
        }

        /// <summary>
        /// Generates a new access token using a valid refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token used to obtain a new access token.</param>
        /// <returns>A LoginResponse containing the new access token, the original refresh token, and its expiry date.</returns>
        /// <exception cref="BusinessException">Thrown when the refresh token is invalid, revoked, or expired.</exception>
        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
        {
            var token = await _userRepository.GetRefreshTokenAsync(refreshToken);

            if (token == null || token.IsRevoked || token.ExpiryDate < DateTime.UtcNow)
            {
                throw new BusinessException("Invalid refresh token");
            }

            var user = await _userRepository.GetByIdAsync(token.UserId);
            if (user == null)
            {
                throw new BusinessException("User not found for the given refresh token!");
            }

            var accessToken = _jwtService.GenerateAccessToken(user);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddHours(1)
            };
        }

        /// <summary>
        /// Gets user data by user ID and returns a RegisterUserResponse containing the user's information.
        /// </summary>
        /// <param name="userId">int userId</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task<RegisterUserResponse> GetDataById(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new BusinessException("User not found");
            }
            return new RegisterUserResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
