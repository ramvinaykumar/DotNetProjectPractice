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
                    "SELECT * FROM bbs.Booking WHERE IsCancelled = 0 ORDER BY BookingDate DESC");
        }

        /// <summary>
        /// Asynchronously retrieves a booking by its unique identifier if it is not cancelled.
        /// </summary>
        /// <param name="bookingId">The unique identifier of the booking.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the booking if found and not
        /// cancelled; otherwise, null.</returns>
        public async Task<Booking?> GetByIdAsync(int bookingId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Booking>(
                "SELECT * FROM bbs.Booking WHERE BookingId=@BookingId AND IsCancelled = 0",
                    new { BookingId = bookingId });
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

        /// <summary>
        /// Updates an existing booking record in the database with new details, such as seat count and total amount, while
        /// </summary>
        /// <param name="bookingId">The unique identifier of the booking to cancel.</param>
        /// <param name="connection">The open database connection used to execute the operation.</param>
        /// <param name="transaction">The database transaction within which the operation is executed.</param>
        /// <returns>The number of rows affected by the update.</returns>
        public async Task<int> UpdateAsync(Booking booking, IDbConnection connection, IDbTransaction transaction)
        {
            const string sql = @" UPDATE bbs.Booking
                                     SET SeatCount=@SeatCount,
                                         TotalAmount=@TotalAmount,
                                         ModifiedDate=@ModifiedDate,
                                         ModifiedBy=@ModifiedBy
                                   WHERE BookingId=@BookingId";

            return await connection.ExecuteAsync(sql, booking, transaction);
        }

        /// <summary>
        /// Determines whether a non-cancelled booking exists for the specified passenger and schedule.
        /// </summary>
        /// <param name="passengerId">The identifier of the passenger.</param>
        /// <param name="scheduleId">The identifier of the schedule.</param>
        /// <returns>True if a duplicate booking exists; otherwise, false.</returns>
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

        /// <summary>
        /// Cancels a booking by updating its status to 'Cancelled' in the database.
        /// </summary>
        /// <param name="bookingId">The unique identifier of the booking to cancel.</param>
        /// <param name="connection">The open database connection used to execute the operation.</param>
        /// <param name="transaction">The database transaction within which the operation is executed.</param>
        /// <returns>The number of rows affected by the update.</returns>
        public async Task<int> CancelAsync(int bookingId, IDbConnection connection, IDbTransaction transaction)
        {
            const string sql = @"UPDATE bbs.Booking
                                    SET BookingStatus = 'Cancelled', IsCancelled = 1, ModifiedDate = GETUTCDATE()
                                  WHERE BookingId=@BookingId";

            return await connection.ExecuteAsync(
                sql,
                new
                {
                    BookingId = bookingId
                },
                transaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> ExistsAsync(int bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
