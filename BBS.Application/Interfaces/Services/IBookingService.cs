using BBS.Application.DTOs.Booking;

namespace BBS.Application.Interfaces.Services
{
    /// <summary>
    /// Defines operations for managing bookings, including creation and retrieval.
    /// </summary>
    /// <remarks>Implementations should provide asynchronous methods for handling booking-related
    /// actions.</remarks>
    public interface IBookingService
    {
        /// <summary>
        /// Creates a new booking based on the provided request data, which includes schedule ID, passenger ID, seat count, and other relevant information. 
        /// The method returns a response containing details of the created booking, such as booking ID, total amount, booking status, and booking date.
        /// </summary>
        /// <param name="request">Booking request object</param>
        /// <returns>Containing the newly added of bookings.</returns>
        Task<BookingResponse> CreateAsync(CreateBookingRequest request);

        /// <summary>
        /// Retrieves all booking records.
        /// </summary>
        /// <returns>Containing the collection of bookings.</returns>
        Task<IEnumerable<BookingResponse>> GetAllAsync();


        //Task<int> CreateBookingAsync(CreateBookingRequest request);

        //Task<IEnumerable<BookingResponse>> GetAllBookingsAsync();

        //Task UpdateBookingAsync(int id, UpdateBookingRequest request);

        //Task DeleteBookingAsync(int id);

        //Task<BookingResponse> GetBookingByIdAsync(int id);

        //Task<BookingResponse> UpdateAsync(int bookingId, UpdateBookingRequest request);

        //Task CancelAsync(int bookingId);

        //Task<BookingResponse> GetByIdAsync(int bookingId);
    }
}
