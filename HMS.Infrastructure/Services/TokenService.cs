using HMS.Core.Interfaces;
using HMS.Core.Models.Staffs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HMS.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config) => _config = config;

        public string GenerateToken(Staff staff)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   staff.StaffId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, staff.Email),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name,               $"{staff.FirstName} {staff.LastName}"),
                new Claim(ClaimTypes.Role,               staff.RoleName),
                new Claim("staffId",                     staff.StaffId.ToString()),
                new Claim("roleId",                      staff.RoleId.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: GetExpiry(),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DateTime GetExpiry()
        {
            var minutes = int.Parse(_config["JwtSettings:ExpiryMinutes"] ?? "480");
            return DateTime.UtcNow.AddMinutes(minutes);
        }
    }
}
