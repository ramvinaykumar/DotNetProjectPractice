using HMS.Core.Dtos.Request.Bookings;
using HMS.Core.Models;
using HMS.Core.Models.Payments;

namespace HMS.Core.Interfaces
{
    public interface IBookingRepository
    {
        Task<(IEnumerable<Booking> Bookings, int TotalCount)> GetAllAsync(BookingQueryRequest query);

        Task<Booking?> GetByIdAsync(int bookingId);

        Task<(Booking? Booking, IEnumerable<Payment> Payments)> GetByIdWithPaymentsAsync(int bookingId);

        Task<int> CreateAsync(BookingCreateRequest request);

        Task<int> UpdateAsync(int bookingId, BookingUpdateRequest request);

        Task<int> CheckInAsync(int bookingId, int? staffId);

        Task<int> CheckOutAsync(int bookingId);

        Task<int> CancelAsync(int bookingId, string? reason);
    }
}
