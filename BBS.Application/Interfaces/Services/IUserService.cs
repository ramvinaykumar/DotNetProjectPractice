using BBS.Application.DTOs.Users;

namespace BBS.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<int> RegisterAsync(RegisterUserRequest request);

        Task<LoginResponse>   LoginAsync(    LoginRequest request);

        Task<LoginResponse> RefreshTokenAsync(  string refreshToken);
    }
}
