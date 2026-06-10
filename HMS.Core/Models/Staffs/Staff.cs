namespace HMS.Core.Models.Staffs
{
    public class Staff
    {
        public int StaffId { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }
        public decimal? Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }
        // From join
        public string RoleName { get; set; } = string.Empty;
        public string? RoleDescription { get; set; }
    }
}
