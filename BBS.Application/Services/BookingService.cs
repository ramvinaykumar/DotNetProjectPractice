using BBS.Application.Common;
using BBS.Application.DTOs;
using BBS.Application.Interfaces;
using BBS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BBS.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;

        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IBookingRepository repository,
            ILogger<BookingService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<int>
            CreateBookingAsync(
            CreateBookingRequest request)
        {
            var seatBooked =
                await _repository
                    .GetByScheduleAndSeatAsync(
                        request.ScheduleId,
                        request.SeatNumber);

            if (seatBooked != null)
                throw new BusinessException(
                    "Seat already booked");

            var booking = new Booking
            {
                ScheduleId = request.ScheduleId,
                PassengerId = request.PassengerId,
                SeatNumber = request.SeatNumber,
                BookingDate = DateTime.UtcNow,
                Status = "Booked"
            };

            return await _repository
                .CreateBookingAsync(booking);
        }

        public async Task<IEnumerable<BookingResponse>>
            GetAllBookingsAsync()
        {
            var result =
                await _repository.GetAllBookingsAsync();

            return result.Select(x =>
                new BookingResponse
                {
                    BookingId = x.BookingId,
                    SeatNumber = x.SeatNumber,
                    Status = x.Status
                });
        }

        public async Task UpdateBookingAsync(
            int id,
            UpdateBookingRequest request)
        {
            var booking =
                await _repository
                    .GetBookingByIdAsync(id);

            if (booking == null)
                throw new BusinessException("Booking not found");

            booking.SeatNumber = request.SeatNumber;
            booking.Status = request.Status;

            await _repository.UpdateBookingAsync(
                booking);
        }

        public async Task DeleteBookingAsync(int id)
        {
            await _repository.DeleteBookingAsync(id);
        }
    }
}
