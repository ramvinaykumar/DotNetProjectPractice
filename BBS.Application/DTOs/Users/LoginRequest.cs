namespace BBS.Application.DTOs.Users
{
    /// <summary>
    /// LoginRequest represents the request data required for user login, including email and password.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Email is the email address of the user attempting to log in.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password is the password of the user attempting to log in.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
