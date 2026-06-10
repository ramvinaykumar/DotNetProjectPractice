using Library.Management.CodeFirst.API.DTOs;

namespace Library.Management.CodeFirst.API.Repositories
{
    /// <summary>
    /// Interface for user repository to handle user-related data operations.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="userDto">The user data transfer object containing user details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the registered user data transfer object.</returns>
        Task<UserDTO> RegisterUserAsync(UserDTO userDto);

        /// <summary>
        /// Retrieves a user by their ID asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user data transfer object.</returns>
        Task<UserDTO> GetUserByIdAsync(int userId);

        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of user data transfer objects.</returns>
        Task<List<UserDTO>> GetAllUsersAsync();
    }
}
