using AutoMapper;
using Library.Management.CodeFirst.API.Data;
using Library.Management.CodeFirst.API.DTOs;
using Library.Management.CodeFirst.API.Models;
using Library.Management.CodeFirst.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Management.CodeFirst.API.Services
{
    public class UserService : IUserRepository
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public UserService(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDTO> RegisterUserAsync(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }
    }
}
