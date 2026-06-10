using Library.Management.CodeFirst.API.Models;

namespace Library.Management.CodeFirst.API.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }
    }
}
