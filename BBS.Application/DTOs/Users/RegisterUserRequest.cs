namespace BBS.Application.DTOs.Users
{
    /// <summary>
    /// RegisterUserRequest represents the request data required for user registration, including username, email, password, and role.
    /// </summary>
    public class RegisterUserRequest
    {
        /// <summary>
        /// UserName is the username of the user attempting to register.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Email is the email address of the user attempting to register.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password is the password of the user attempting to register.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Role is the role assigned to the user attempting to register, such as "Admin" or "User".
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}
