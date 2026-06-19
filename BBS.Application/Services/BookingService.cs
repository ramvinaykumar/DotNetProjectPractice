using BBS.Application.Common;
using BBS.Application.DTOs.Booking;
using BBS.Application.Interfaces.Infrastructure;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Services;
using BBS.Domain.Constants;
using BBS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BBS.Application.Services
{
    /// <summary>
    /// Provides operations for creating and retrieving bookings, ensuring validation of passenger and schedule data.
    /// </summary>
    /// <remarks>Interacts with booking, passenger, and schedule repositories to manage booking transactions
    /// and maintain data integrity.</remarks>
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<BookingService> _logger;

        /// <summary>
        /// Parameterized constructor for initializing the BookingService with necessary dependencies, including repositories for bookings, passengers, 
        /// and schedules, a database connection factory for managing transactions, and a logger for tracking operations and errors.
        /// </summary>
        /// <param name="repository">IBookingRepository repository</param>
        /// <param name="passengerRepository">IPassengerRepository passengerRepository</param>
        /// <param name="scheduleRepository">IScheduleRepository scheduleRepository</param>
        /// <param name="connectionFactory">IDbConnectionFactory connectionFactory</param>
        /// <param name="logger">ILogger<BookingService> logger</param>
        public BookingService(IBookingRepository repository, IPassengerRepository passengerRepository
            , IScheduleRepository scheduleRepository, IDbConnectionFactory connectionFactory
            , ILogger<BookingService> logger)
        {
            _bookingRepository = repository;
            _passengerRepository = passengerRepository;
            _scheduleRepository = scheduleRepository;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new booking for a passenger on a specified schedule.
        /// </summary>
        /// <param name="request">The booking request containing passenger ID, schedule ID, and seat count.</param>
        /// <returns>A BookingResponse with details of the created booking.</returns>
        /// <exception cref="BusinessException">Thrown if the passenger is not found, inactive, has already booked the schedule, if the schedule is not
        /// found, or if there are insufficient available seats.</exception>
        public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
        {
            _logger.LogInformation("Booking request started. PassengerId:{PassengerId}, ScheduleId:{ScheduleId}", request.PassengerId, request.ScheduleId);

            // Validate Passenger
            var passenger = await _passengerRepository.GetByIdAsync(request.PassengerId);

            if (passenger == null)
            {
                throw new BusinessException("Passenger not found.");
            }

            if (!passenger.IsActive)
            {
                throw new BusinessException("Passenger is inactive.");
            }

            // Validate Schedule
            var schedule = await _scheduleRepository.GetByIdAsync(request.ScheduleId);

            if (schedule == null)
            {
                throw new BusinessException("Schedule not found.");
            }

            if (schedule.AvailableSeats < request.SeatCount)
            {
                throw new BusinessException("Requested seats are not available.");
            }

            // Validate Available Seats
            if (schedule.AvailableSeats < request.SeatCount)
            {
                throw new BusinessException("Requested seats are not available.");
            }

            // Duplicate Booking Check
            bool duplicateBooking = await _bookingRepository
                    .HasDuplicateBookingAsync(
                        request.PassengerId,
                        request.ScheduleId);

            if (duplicateBooking)
            {
                throw new BusinessException("Passenger already booked this schedule.");
            }

            using var connection = _connectionFactory.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var booking = new Booking
                {
                    PassengerId = request.PassengerId,
                    ScheduleId = request.ScheduleId,
                    SeatCount = request.SeatCount,
                    TotalAmount = request.SeatCount * schedule.Fare,
                    BookingStatus = BookingStatus.Confirmed,
                    BookingDate = DateTime.UtcNow,
                    IsCancelled = false,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "SYSTEM"
                };

                // Create Booking
                int bookingId = await _bookingRepository.CreateAsync(
                            booking,
                            connection,
                            transaction);

                // Deduct Seats
                var rowsAffected = await _scheduleRepository.UpdateAvailableSeatsAsync(
                        request.ScheduleId,
                        -request.SeatCount,
                        connection,
                        transaction);

                if (rowsAffected == 0)
                {
                    throw new BusinessException("Not enough seats available.");
                }

                transaction.Commit();

                _logger.LogInformation("Booking created successfully. BookingId {BookingId}", bookingId);

                return new BookingResponse
                {
                    BookingId = bookingId,
                    PassengerId = booking.PassengerId,
                    ScheduleId = booking.ScheduleId,
                    SeatCount = booking.SeatCount,
                    TotalAmount = booking.TotalAmount,
                    BookingStatus = booking.BookingStatus,
                    BookingDate = booking.BookingDate
                };
            }
            catch (Exception ex)
            {
                if (transaction.Connection != null)
                {
                    transaction.Rollback();
                }

                _logger.LogError(ex, "Booking creation failed for PassengerId:{PassengerId}", request.PassengerId);
                throw;
            }
        }

        /// <summary>
        /// Asynchronously retrieves all bookings.
        /// </summary>
        /// <returns>A collection of booking responses.</returns>
        public async Task<IEnumerable<BookingResponse>> GetAllAsync()
        {
            var result = await _bookingRepository.GetAllAsync();
            return result.Select(booking => new BookingResponse
            {
                BookingId = booking.BookingId,
                PassengerId = booking.PassengerId,
                ScheduleId = booking.ScheduleId,
                SeatCount = booking.SeatCount,
                TotalAmount = booking.TotalAmount,
                BookingStatus = booking.BookingStatus,
                BookingDate = booking.BookingDate
            });
        }

        //public async Task UpdateBookingAsync(int id, UpdateBookingRequest request)
        //{
        //    var booking = await _repository
        //            .GetBookingByIdAsync(id);

        //    if (booking == null)
        //        throw new BusinessException("Booking not found");

        //    booking.SeatNumber = request.SeatCount;
        //    booking.Status = request.Status;

        //    await _repository.UpdateBookingAsync(booking);
        //}

        //public async Task DeleteBookingAsync(int id)
        //{
        //    await _repository.DeleteBookingAsync(id);
        //}

        //public async Task<BookingResponse> GetBookingByIdAsync(int id)
        //{
        //    var booking = await _repository.GetBookingByIdAsync(id);
        //    if (booking == null)
        //        throw new BusinessException("Booking not found");
        //    return new BookingResponse
        //    {
        //        BookingId = booking.BookingId,
        //        SeatNumber = booking.SeatNumber,
        //        Status = booking.Status
        //    };
        //}
    }
}
