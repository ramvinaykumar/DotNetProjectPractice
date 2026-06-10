using BBS.Application.DTOs;

namespace BBS.Application.Interfaces
{
    public interface IBookingService
    {
        Task<int> CreateBookingAsync(CreateBookingRequest request);

        Task<IEnumerable<BookingResponse>> GetAllBookingsAsync();

        Task UpdateBookingAsync(int id, UpdateBookingRequest request);

        Task DeleteBookingAsync(int id);
    }
}
