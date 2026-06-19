using BBS.Application.DTOs.Schedule;
using System.Threading.Tasks;

namespace BBS.Application.Interfaces.Services
{
    /// <summary>
    /// Defines operations for creating, retrieving, updating, and deleting schedule entries.
    /// </summary>
    /// <remarks>Intended for managing schedules within the application, supporting asynchronous operations
    /// for schedule lifecycle management.</remarks>
    public interface IScheduleService
    {
        /// <summary>
        /// Asynchronously creates a new schedule.
        /// </summary>
        /// <param name="request">The details of the schedule to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response for the created
        /// schedule.</returns>
        Task<ScheduleResponse> CreateAsync(CreateScheduleRequest request);

        /// <summary>
        /// Asynchronously retrieves all schedule entries.
        /// </summary>
        /// <returns>A collection of ScheduleResponse objects representing the schedules.</returns>
        Task<IEnumerable<ScheduleResponse>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves a schedule by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the schedule.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the schedule response if found;
        /// otherwise, null.</returns>
        Task<ScheduleResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing schedule with the specified values.
        /// </summary>
        /// <param name="id">The unique identifier of the schedule to update.</param>
        /// <param name="request">The updated schedule information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="BusinessException">Thrown when a schedule with the specified identifier does not exist.</exception>
        Task<ScheduleResponse> UpdateAsync(int id, UpdateScheduleRequest request);

        /// <summary>
        /// Asynchronously deletes a schedule entry by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the schedule entry to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        Task DeleteAsync(int id);
    }
}
