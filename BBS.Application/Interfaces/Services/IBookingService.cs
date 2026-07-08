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

        /// <summary>
        /// Retrieves a booking by its unique identifier.
        /// </summary>
        /// <param name="bookingId">The unique identifier of the booking to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing the booking response.</returns>
        Task<BookingResponse> GetByIdAsync(int bookingId);

        /// <summary>
        /// Updates an existing booking with the specified information.
        /// </summary>
        /// <param name="bookingId">The unique identifier of the booking to update.</param>
        /// <param name="request">The updated booking details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated booking response.</returns>
        Task<BookingResponse> UpdateBookingAsync(int bookingId, UpdateBookingRequest request);

        /// <summary>
        /// Cancels an existing booking by its unique identifier, marking it as cancelled in the system. 
        /// The method returns a task that represents the asynchronous operation, and the task result indicates whether the cancellation was successful.
        /// </summary>
        /// <param name="bookingId">The unique identifier of the booking to retrieve.</param>
        /// <returns></returns>
        Task CancelAsync(int bookingId);
    }
}
