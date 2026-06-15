using BBS.Domain.Entities;

namespace BBS.Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface for bus repository, providing methods for creating, retrieving, updating, and deleting bus records.
    /// </summary>
    public interface IBusRepository
    {
        /// <summary>
        /// Creates a new bus record in the database.
        /// </summary>
        /// <param name="bus">Bus bus</param>
        /// <returns>Returns newly added busId data if successfully added else will return error message.</returns>
        Task<int> CreateAsync(Bus bus);

        /// <summary>
        /// Gets all bus records from the database.
        /// </summary>
        /// <returns>Return all bus records from the database else will return error message.</returns>
        Task<IEnumerable<Bus>> GetAllAsync();

        /// <summary>
        /// Gets a bus record by its ID from the database.
        /// </summary>
        /// <param name="busId">int busId</param>
        /// <returns>Returns a bus data if successfully fetched else will return error message.</returns>
        Task<Bus?> GetByIdAsync(int busId);

        /// <summary>
        /// Updates an existing bus record in the database.
        /// </summary>
        /// <param name="bus">Bus bus</param>
        /// <returns>Returns updated busid data if successfully update else will return error message.</returns>
        Task<int> UpdateAsync(Bus bus);

        /// <summary>
        /// Deletes a bus record from the database by its ID.
        /// </summary>
        /// <param name="busId">int busId</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
        Task<int> DeleteAsync(int busId);
    }
}
