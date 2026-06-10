using HMS.Core.Dtos.Request.Staff;
using HMS.Core.Models.Staffs;

namespace HMS.Core.Interfaces
{
    public interface IStaffRepository
    {
        Task<IEnumerable<Staff>> GetAllAsync(int? roleId, bool? isActive);

        Task<Staff?> GetByIdAsync(int staffId);

        Task<Staff?> GetByEmailAsync(string email);

        Task<int> CreateAsync(StaffCreateRequest request, string passwordHash);

        Task<int> UpdateAsync(int staffId, StaffUpdateRequest request);

        Task<int> UpdatePasswordAsync(int staffId, string passwordHash);

        Task<IEnumerable<Role>> GetRolesAsync();
    }
}
