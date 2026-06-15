using BBS.Domain.Entities;

namespace BBS.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByIdAsync(int userId);

        Task<int> CreateUserAsync(User user);

        Task<bool> UserExistsAsync(string email);

        Task SaveRefreshTokenAsync(RefreshToken refreshToken);

        Task<RefreshToken?> GetRefreshTokenAsync(string token);

        Task RevokeRefreshTokenAsync(string token);
    }
}
