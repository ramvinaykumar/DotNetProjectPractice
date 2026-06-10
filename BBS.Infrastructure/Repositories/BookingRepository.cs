using BBS.Application.Interfaces;
using BBS.Domain.Entities;
using BBS.Infrastructure.ConnectionFactory;
using Dapper;

namespace BBS.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BookingRepository(
            IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int>
            CreateBookingAsync(
            Booking booking)
        {
            const string sql = @"

        INSERT INTO Booking
        (
            ScheduleId,
            PassengerId,
            SeatNumber,
            BookingDate,
            Status
        )

        VALUES
        (
            @ScheduleId,
            @PassengerId,
            @SeatNumber,
            @BookingDate,
            @Status
        )

        SELECT CAST(
            SCOPE_IDENTITY()
            AS INT)
        ";

            using var connection =
                _connectionFactory.CreateConnection();

            return await connection
                .QuerySingleAsync<int>(
                    sql,
                    booking);
        }

        public async Task<IEnumerable<Booking>>
            GetAllBookingsAsync()
        {
            using var connection =
                _connectionFactory.CreateConnection();

            return await connection
                .QueryAsync<Booking>(
                    "SELECT * FROM Booking");
        }

        public async Task<Booking?>
            GetBookingByIdAsync(int id)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            return await connection
                .QueryFirstOrDefaultAsync<Booking>(
                    "SELECT * FROM Booking WHERE BookingId=@Id",
                    new { Id = id });
        }

        public async Task<Booking?>
            GetByScheduleAndSeatAsync(
            int scheduleId,
            int seatNo)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            return await connection
                .QueryFirstOrDefaultAsync<Booking>(
                    @"SELECT * FROM Booking
                  WHERE ScheduleId=@ScheduleId
                  AND SeatNumber=@SeatNo",
                    new
                    {
                        ScheduleId = scheduleId,
                        SeatNo = seatNo
                    });
        }

        public async Task<int>
            UpdateBookingAsync(
            Booking booking)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                @"UPDATE Booking
              SET SeatNumber=@SeatNumber,
                  Status=@Status
              WHERE BookingId=@BookingId",
                booking);
        }

        public async Task<int>
            DeleteBookingAsync(int id)
        {
            using var connection =
                _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                "DELETE FROM Booking WHERE BookingId=@Id",
                new { Id = id });
        }
    }
}
