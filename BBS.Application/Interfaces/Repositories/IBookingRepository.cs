using BBS.Domain.Entities;
using System.Data;

namespace BBS.Application.Interfaces.Repositories
{
    /// <summary>
    /// Defines a contract for managing booking entities, including creation, retrieval, update, cancellation, and
    /// existence checks.
    /// </summary>
    /// <remarks>Provides asynchronous operations for handling bookings in a data store, supporting
    /// transactional operations and duplicate detection.</remarks>
    public interface IBookingRepository
    {
        /// <summary>
        /// Creates a new booking asynchronously, using the provided database connection and transaction for the operation.
        /// </summary>
        /// <param name="booking">Booking booking</param>
        /// <param name="connection">IDbConnection connection</param>
        /// <param name="transaction">IDbTransaction transaction</param>
        /// <returns>Returns integer value of newly created data</returns>
        Task<int> CreateAsync(Booking booking, IDbConnection connection, IDbTransaction transaction);

        /// <summary>
        /// Gets all bookings asynchronously.
        /// </summary>
        /// <returns>Returns all bookings</returns>
        Task<IEnumerable<Booking>> GetAllAsync();

        /// <summary>
        /// Gets a booking by its unique identifier asynchronously.
        /// </summary>
        /// <param name="bookingId">int bookingId</param>
        /// <returns>Return booking data</returns>
        Task<Booking?> GetByIdAsync(int bookingId);

        /// <summary>
        /// Determines whether a booking already exists for the specified passenger and schedule.
        /// </summary>
        /// <param name="passengerId">The unique identifier of the passenger.</param>
        /// <param name="scheduleId">The unique identifier of the schedule.</param>
        /// <returns>True if a duplicate booking exists; otherwise, false.</returns>
        Task<bool> HasDuplicateBookingAsync(int passengerId, int scheduleId);

        /// <summary>
        /// Updates an existing booking asynchronously, using the provided database connection and transaction for the operation.
        /// </summary>
        /// <param name="booking">Booking booking</param>
        /// <param name="connection">IDbConnection connection</param>
        /// <param name="transaction">IDbTransaction transaction</param>
        /// <returns></returns>
        Task<int> UpdateAsync(Booking booking, IDbConnection connection, IDbTransaction transaction);

        /// <summary>
        /// Cancels a booking asynchronously.
        /// </summary>
        /// <param name="bookingId">The identifier of the booking to cancel.</param>
        /// <param name="connection">The database connection to use for the operation.</param>
        /// <param name="transaction">The database transaction to use for the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
        Task<int> CancelAsync(int bookingId, IDbConnection connection, IDbTransaction transaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(int bookingId);
    }
}
