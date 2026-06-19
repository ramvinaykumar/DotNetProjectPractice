using BBS.Domain.Entities;
using System.Data;

namespace BBS.Application.Interfaces.Repositories
{
    public interface IScheduleRepository
    {
        /// <summary>
        /// Asynchronously inserts a new bus schedule into the database and returns the generated identifier.
        /// </summary>
        /// <param name="schedule">The bus schedule to insert.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the identifier of the newly
        /// created bus schedule.</returns>
        Task<int> CreateAsync(BusSchedule schedule);

        /// <summary>
        /// Asynchronously retrieves all bus schedules from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of bus schedules.</returns>
        Task<IEnumerable<BusSchedule>> GetAllAsync();

        /// <summary>
        /// Gets a bus schedule by its unique identifier asynchronously.
        /// </summary>
        /// <param name="scheduleId">int scheduleId</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
        Task<BusSchedule?> GetByIdAsync(int scheduleId);

        /// <summary>
        /// Asynchronously updates a bus schedule in the database.
        /// </summary>
        /// <param name="schedule">The bus schedule entity containing updated values.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
        Task<int> UpdateAsync(BusSchedule schedule);

        /// <summary>
        /// Asynchronously deletes the bus schedule with the specified identifier.
        /// </summary>
        /// <param name="scheduleId">The unique identifier of the bus schedule to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
        Task<int> DeleteAsync(int scheduleId);

        /// <summary>
        /// Asynchronously determines whether a bus schedule exists for the specified bus ID and departure time.
        /// </summary>
        /// <param name="busId">The unique identifier of the bus.</param>
        /// <param name="departureTime">The departure time to check for the bus schedule.</param>
        /// <returns>True if a matching schedule exists; otherwise, false.</returns>
        Task<bool> ScheduleExistsAsync(int busId, DateTime departureTime);

        /// <summary>
        /// Determines whether the specified bus has a scheduling conflict within the given time range.
        /// </summary>
        /// <param name="busId">The unique identifier of the bus.</param>
        /// <param name="departureTime">The scheduled departure time.</param>
        /// <param name="arrivalTime">The scheduled arrival time.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if a scheduling conflict
        /// exists; otherwise, false.</returns>
        Task<bool> HasScheduleConflictAsync(int busId, DateTime departureTime, DateTime arrivalTime);

        Task<int> UpdateAvailableSeatsAsync(int scheduleId, int seatCount, IDbConnection connection, IDbTransaction transaction);

        Task<int> DeductSeatsAsync(int scheduleId, int seatsToDeduct, IDbConnection connection, IDbTransaction transaction);
    }
}
