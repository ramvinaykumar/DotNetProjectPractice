using BBS.Application.DTOs.Booking;

namespace BBS.Application.Interfaces.Services
{
    public interface IBookingService
    {
        Task<int> CreateBookingAsync(CreateBookingRequest request);

        Task<IEnumerable<BookingResponse>> GetAllBookingsAsync();

        Task UpdateBookingAsync(int id, UpdateBookingRequest request);

        Task DeleteBookingAsync(int id);
    }
}
