using BBS.Application.DTOs.Users;

namespace BBS.Application.Interfaces.Services
{
    /// <summary>
    /// Defines operations for user registration, authentication, and token management.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Asynchronously registers a new user.
        /// </summary>
        /// <param name="request">The registration details for the new user.</param>
        /// <returns>A task representing the asynchronous operation, containing the registration response.</returns>
        Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request);

        /// <summary>
        /// Logins a user asynchronously using the provided credentials.
        /// </summary>
        /// <param name="request">LoginRequest request</param>
        /// <returns>A task representing the asynchronous operation, containing the login response.</returns>
        Task<LoginResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Refreshes the access token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">string refreshToken</param>
        /// <returns>A task representing the asynchronous operation, containing the login response.</returns>
        Task<LoginResponse> RefreshTokenAsync(string refreshToken);
    }
}
