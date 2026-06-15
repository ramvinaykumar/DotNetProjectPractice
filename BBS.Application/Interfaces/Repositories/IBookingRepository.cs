using BBS.Domain.Entities;

namespace BBS.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<int> CreateBookingAsync(Booking booking);

        Task<IEnumerable<Booking>> GetAllBookingsAsync();

        Task<Booking?> GetBookingByIdAsync(int id);

        Task<Booking?> GetByScheduleAndSeatAsync(int scheduleId, int seatNo);

        Task<int> UpdateBookingAsync(Booking booking);

        Task<int> DeleteBookingAsync(int id);
    }
}
