using BBS.Domain.Entities;

namespace BBS.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();
    }
}
