using BBS.Domain.Entities;

namespace BBS.Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface for managing passenger data, providing methods for CRUD operations and validation checks on email and phone number uniqueness.
    /// </summary>
    public interface IPassengerRepository
    {
        /// <summary>
        /// Gets all passengers asynchronously.
        /// </summary>
        /// <returns>Returns all active passengers</returns>
        Task<IEnumerable<Passenger>> GetAllAsync();

        /// <summary>
        /// Creates a new passenger asynchronously and returns the ID of the newly created passenger.
        /// </summary>
        /// <param name="passenger">Passenger passenger</param>
        /// <returns></returns>
        Task<int> CreateAsync(Passenger passenger);

        /// <summary>
        /// Checks if an email address already exists in the repository asynchronously, ensuring uniqueness for passenger records.
        /// </summary>
        /// <param name="email">string email</param>
        /// <returns></returns>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// Checks if a phone number already exists in the repository asynchronously, ensuring uniqueness for passenger records.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<bool> PhoneExistsAsync(string phoneNumber);

        /// <summary>
        /// Asynchronously retrieves a passenger by unique identifier.
        /// </summary>
        /// <param name="passengerId">The unique identifier of the passenger.</param>
        /// <returns>A task representing the asynchronous operation, containing the passenger if found; otherwise, null.</returns>
        Task<Passenger?> GetByIdAsync(int passengerId);

        /// <summary>
        /// Asynchronously updates the specified passenger's details.
        /// </summary>
        /// <param name="passenger">The passenger entity containing updated information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of records affected.</returns>
        Task<int> UpdateAsync(Passenger passenger);

        /// <summary>
        /// Deletes a passenger by unique identifier asynchronously, removing the passenger from the repository.
        /// </summary>
        /// <param name="passengerId">The unique identifier of the passenger.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of records affected.</returns>
        Task<int> DeleteAsync(int passengerId);
    }
}
