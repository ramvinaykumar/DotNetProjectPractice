using HMS.Core.Models.Staffs;

namespace HMS.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Staff staff);

        DateTime GetExpiry();
    }
}
