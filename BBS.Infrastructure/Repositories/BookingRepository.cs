using BBS.Application.Interfaces.Infrastructure;
using BBS.Application.Interfaces.Repositories;
using BBS.Domain.Entities;
using Dapper;
using System.Data;

namespace BBS.Infrastructure.Repositories
{
    /// <summary>
    /// Provides data access operations for managing bookings, including creation, retrieval, updating, deletion, and
    /// duplicate checks.
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Parameterized constructor for initializing the BookingRepository with a database connection factory, enabling
        /// </summary>
        /// <param name="connectionFactory">IDbConnectionFactory connectionFactory</param>
        public BookingRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Inserts a new booking record into the database and returns the generated identifier.
        /// </summary>
        /// <param name="booking">The booking details to insert.</param>
        /// <param name="connection">The database connection to use.</param>
        /// <param name="transaction">The transaction to associate with the operation.</param>
        /// <returns>The identifier of the newly created booking record.</returns>
        public async Task<int> CreateAsync(Booking booking, IDbConnection connection, IDbTransaction transaction)
        {
            const string sql = @"INSERT INTO bbs.Booking
                                (
                                    PassengerId,
                                    ScheduleId,
                                    SeatCount,
                                    TotalAmount,
                                    BookingStatus,
                                    BookingDate,
                                    IsCancelled,
                                    CreatedDate,
                                    CreatedBy
                                )
                                VALUES
                                (
                                    @PassengerId,
                                    @ScheduleId,
                                    @SeatCount,
                                    @TotalAmount,
                                    @BookingStatus,
                                    @BookingDate,
                                    @IsCancelled,
                                    @CreatedDate,
                                    @CreatedBy
                                )

                                SELECT CAST( SCOPE_IDENTITY() AS INT)";

            return await connection.QuerySingleAsync<int>(sql, booking, transaction);
        }

        /// <summary>
        /// Asynchronously retrieves all bookings that have not been cancelled.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of non-cancelled
        /// bookings.</returns>
        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Booking>(
                    "SELECT * FROM bbs.Booking WHERE IsCancelled = 0");
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QueryFirstOrDefaultAsync<Booking>("SELECT * FROM bbs.Booking WHERE BookingId=@Id",
                    new { Id = id });
        }

        public async Task<Booking?> GetByScheduleAndSeatAsync(int scheduleId, int seatNo)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Booking>(
                    @"SELECT * FROM bbs.Booking
                       WHERE ScheduleId=@ScheduleId
                         AND SeatNumber=@SeatNo",
                            new
                            {
                                ScheduleId = scheduleId,
                                SeatNo = seatNo
                            });
        }

        public async Task<int> UpdateBookingAsync(Booking booking)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                                @"UPDATE bbs.Booking
                                     SET SeatNumber=@SeatNumber,
                                         Status=@Status
                                   WHERE BookingId=@BookingId", booking);
        }

        public async Task<int> DeleteBookingAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                "DELETE FROM bbs.Booking WHERE BookingId=@Id",
                new { Id = id });
        }

        public async Task<bool> HasDuplicateBookingAsync(int passengerId, int scheduleId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT COUNT(*)
                                   FROM bbs.Booking
                                  WHERE PassengerId=@PassengerId
                                    AND ScheduleId=@ScheduleId
                                    AND IsCancelled=0";

            int count = await connection.ExecuteScalarAsync<int>(
                    sql,
                    new
                    {
                        PassengerId = passengerId,
                        ScheduleId = scheduleId
                    });

            return count > 0;
        }

        public Task<int> Cr44eateAsync(Booking booking, IDbConnection connection, IDbTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Booking booking)
        {
            throw new NotImplementedException();
        }

        public Task<int> CancelAsync(int bookingId)
        {
            throw new NotImplementedException();
        }

        public Task<Booking?> GetByIdAsync(int bookingId)
        {
            throw new NotImplementedException();
        }
        

        public Task<bool> ExistsAsync(int bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
