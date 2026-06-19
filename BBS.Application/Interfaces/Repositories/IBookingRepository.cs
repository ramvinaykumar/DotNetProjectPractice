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
        Task<int> CreateAsync(Booking booking, IDbConnection connection, IDbTransaction transaction);

        Task<IEnumerable<Booking>> GetAllAsync();

        Task<bool> HasDuplicateBookingAsync(int passengerId, int scheduleId);

        Task<int> UpdateAsync(Booking booking);

        Task<int> CancelAsync(int bookingId);

        Task<Booking?> GetByIdAsync(int bookingId);

        Task<bool> ExistsAsync(int bookingId);        
    }
}
