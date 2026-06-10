using Dapper;
using HMS.Core.Dtos.Request.Staff;
using HMS.Core.Interfaces;
using HMS.Core.Models.Staffs;
using HMS.Infrastructure.Data;
using System.Data.Common;

namespace HMS.Infrastructure.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly IDbConnectionFactory _db;
        public StaffRepository(IDbConnectionFactory db) => _db = db;
        private DbConnection Conn() => (DbConnection)_db.CreateConnection();

        public async Task<IEnumerable<Staff>> GetAllAsync(int? roleId, bool? isActive)
        {
            using var conn = Conn();
            return await conn.QueryAsync<Staff>("hr.usp_Staff_GetAll",
                new { RoleId = roleId, IsActive = isActive },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Staff?> GetByIdAsync(int id)
        {
            using var conn = Conn();
            return await conn.QueryFirstOrDefaultAsync<Staff>("hr.usp_Staff_GetById",
                new { StaffId = id },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Staff?> GetByEmailAsync(string email)
        {
            using var conn = Conn();
            return await conn.QueryFirstOrDefaultAsync<Staff>("hr.usp_Staff_GetByEmail",
                new { Email = email },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> CreateAsync(StaffCreateRequest r, string passwordHash)
        {
            using var conn = Conn();
            return await conn.QueryFirstAsync<int>("hr.usp_Staff_Create",
                new
                {
                    r.RoleId,
                    r.FirstName,
                    r.LastName,
                    r.Email,
                    r.PhoneNumber,
                    PasswordHash = passwordHash,
                    r.Salary,
                    r.HireDate
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(int id, StaffUpdateRequest r)
        {
            using var conn = Conn();
            return await conn.QueryFirstAsync<int>("hr.usp_Staff_Update",
                new
                {
                    StaffId = id,
                    r.RoleId,
                    r.FirstName,
                    r.LastName,
                    r.Email,
                    r.PhoneNumber,
                    r.Salary,
                    r.IsActive
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> UpdatePasswordAsync(int id, string passwordHash)
        {
            using var conn = Conn();
            return await conn.QueryFirstAsync<int>("hr.usp_Staff_UpdatePassword",
                new { StaffId = id, PasswordHash = passwordHash },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            using var conn = Conn();
            return await conn.QueryAsync<Role>(
                "SELECT RoleId, RoleName, Description, IsActive FROM hr.Roles WHERE IsActive = 1 ORDER BY RoleName");
        }
    }
}
