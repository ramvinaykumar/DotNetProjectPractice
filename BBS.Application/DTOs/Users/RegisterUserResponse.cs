namespace BBS.Application.DTOs.Users
{
    /// <summary>
    /// RegisterUserResponse represents the response returned after a successful user registration.
    /// </summary>
    public class RegisterUserResponse
    {
        /// <summary>
        /// UserName is the username of the registered user.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Email is the email address of the registered user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// UserId is the unique identifier of the registered user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// UserRole is the role assigned to the registered user like Admin, User, etc.
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}
