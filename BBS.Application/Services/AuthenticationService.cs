using BBS.Application.Common;
using BBS.Application.DTOs.Users;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Services;

namespace BBS.Application.Services
{
    public class AuthenticationService
    {
        private readonly IUserRepository _userRepo;

        private readonly IJwtTokenService _jwtService;

        public AuthenticationService(
            IUserRepository userRepo,
            IJwtTokenService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }

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
