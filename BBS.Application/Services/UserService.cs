using BBS.Application.Common;
using BBS.Application.DTOs.Users;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Services;
using BBS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BBS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IJwtTokenService _jwtService;

        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IJwtTokenService jwtService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<int> RegisterAsync(RegisterUserRequest request)
        {
            var exists = await _userRepository
                    .UserExistsAsync(request.Email);

            if (exists)
            {
                throw new BusinessException("Email already exists");
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role
            };

            return await _userRepository
                .CreateUserAsync(user);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new BusinessException("Invalid credentials");
            }

            bool passwordMatched = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!passwordMatched)
            {
                throw new BusinessException("Invalid credentials");
            }

            var accessToken = _jwtService.GenerateAccessToken(user);

            var refreshToken = _jwtService.GenerateRefreshToken();

            var hashedRefreshToken = BCrypt.Net.BCrypt.HashPassword(refreshToken);

            await _userRepository.SaveRefreshTokenAsync(
                    new RefreshToken
                    {
                        UserId = user.UserId,
                        TokenHash = hashedRefreshToken,
                        ExpiryDate = DateTime.UtcNow.AddDays(7),
                        IsRevoked = false
                    });

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
        {
            var token = await _userRepository
                    .GetRefreshTokenAsync(refreshToken);

            if (token == null || token.IsRevoked || token.ExpiryDate < DateTime.UtcNow)
            {
                throw new BusinessException("Invalid refresh token");
            }

            var user = await _userRepository
                    .GetByIdAsync(token.UserId);

            var accessToken = _jwtService
                    .GenerateAccessToken(user);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}
