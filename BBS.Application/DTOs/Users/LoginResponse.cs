namespace BBS.Application.DTOs.Users
{
    /// <summary>
    /// User login response containing access token, refresh token, and expiry date.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// AccessToken is the JWT access token issued upon successful authentication, used for authorizing subsequent requests.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// RefreshToken is the token used to obtain a new access token when the current one expires, allowing for continued access without re-authentication.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// ExpiryDate indicates the date and time when the access token will expire, after which a new access token must be obtained using the refresh token.
        /// </summary>
        public DateTime ExpiryDate { get; set; }
    }
}
