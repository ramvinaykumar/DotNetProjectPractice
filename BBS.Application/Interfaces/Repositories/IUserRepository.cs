using BBS.Domain.Entities;

namespace BBS.Application.Interfaces.Repositories
{
    /// <summary>
    /// Defines operations for managing users and their refresh tokens. 
    /// Repositories implementing this interface should provide methods for retrieving users by email or ID, 
    /// creating new users, checking for existing users, and managing refresh tokens.
    /// </summary>
    /// <remarks>Provides asynchronous methods for retrieving, creating, and verifying users, as well as
    /// handling refresh token storage and revocation.</remarks>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets a user by their email address asynchronously. Returns null if the user does not exist.
        /// </summary>
        /// <param name="email">string email</param>
        /// <returns>A task representing the asynchronous operation, containing the user if found; otherwise, null.</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Asynchronously retrieves a user by unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, containing the user if found; otherwise, null.</returns>
        Task<User?> GetByIdAsync(int userId);

        /// <summary>
        /// Creates a new user asynchronously and returns the unique identifier of the created user.
        /// </summary>
        /// <param name="user">User user</param>
        /// <returns>Returns newly added userId</returns>
        Task<int> CreateUserAsync(User user);

        /// <summary>
        /// Checks if a user with the specified email exists asynchronously.
        /// </summary>
        /// <param name="email">string email</param>
        /// <returns>Returns true if found else false.</returns>
        Task<bool> UserExistsAsync(string email);

        /// <summary>
        /// Asynchronously saves a refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to save.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);

        /// <summary>
        /// Asynchronously retrieves a refresh token matching the specified token string.
        /// </summary>
        /// <param name="token">The token string used to locate the refresh token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the refresh token if found;
        /// otherwise, null.</returns>
        Task<RefreshToken?> GetRefreshTokenAsync(string token);

        /// <summary>
        /// Revokes a refresh token asynchronously, effectively invalidating it for future use.
        /// </summary>
        /// <param name="token">string token</param>
        /// <returns></returns>
        Task RevokeRefreshTokenAsync(string token);
    }
}
