using BBS.Application.Interfaces.Infrastructure;
using BBS.Application.Interfaces.Repositories;
using BBS.Domain.Entities;
using BBS.Infrastructure.ConnectionFactory;
using Dapper;
using System.Data;

namespace BBS.Infrastructure.Repositories
{
    /// <summary>
    /// Provides methods for performing CRUD operations on bus schedules in the database.
    /// </summary>
    /// <remarks>Interacts with the bbs.BusSchedule table to manage schedule data.</remarks>
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Parameterized constructor that accepts a database connection factory for creating connections to the database.
        /// </summary>
        /// <param name="connectionFactory"></param>
        public ScheduleRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Asynchronously inserts a new bus schedule into the database and returns the generated identifier.
        /// </summary>
        /// <param name="schedule">The bus schedule to insert.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the identifier of the newly
        /// created bus schedule.</returns>
        public async Task<int> CreateAsync(BusSchedule schedule)
        {
            const string sql = @"
                    INSERT INTO bbs.BusSchedule
                    (
                        BusId,
                        RouteId,
                        DepartureTime,
                        ArrivalTime,
                        Fare
                    )
                    VALUES
                    (
                        @BusId,
                        @RouteId,
                        @DepartureTime,
                        @ArrivalTime,
                        @Fare
                    )

                    SELECT CAST(SCOPE_IDENTITY() AS INT)";

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QuerySingleAsync<int>(sql, schedule);
        }

        /// <summary>
        /// Asynchronously retrieves all bus schedules from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of bus schedules.</returns>
        public async Task<IEnumerable<BusSchedule>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<BusSchedule>("SELECT * FROM bbs.BusSchedule");
        }

        /// <summary>
        /// Gets a bus schedule by its unique identifier asynchronously.
        /// </summary>
        /// <param name="scheduleId">int scheduleId</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
        public async Task<BusSchedule?> GetByIdAsync(int scheduleId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QueryFirstOrDefaultAsync<BusSchedule>(
                      @"SELECT *
                          FROM bbs.BusSchedule
                          WHERE ScheduleId=@ScheduleId", new { ScheduleId = scheduleId });
        }

        /// <summary>
        /// Asynchronously updates a bus schedule in the database.
        /// </summary>
        /// <param name="schedule">The bus schedule entity containing updated values.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
        public async Task<int> UpdateAsync(BusSchedule schedule)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                        @"UPDATE bbs.BusSchedule
                             SET DepartureTime=@DepartureTime,
                                 ArrivalTime=@ArrivalTime,
                                 Fare=@Fare
                           WHERE ScheduleId=@ScheduleId", schedule);
        }

        /// <summary>
        /// Asynchronously deletes the bus schedule with the specified identifier.
        /// </summary>
        /// <param name="scheduleId">The unique identifier of the bus schedule to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
        public async Task<int> DeleteAsync(int scheduleId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                @"DELETE FROM bbs.BusSchedule WHERE ScheduleId=@ScheduleId", new { ScheduleId = scheduleId });
        }

        /// <summary>
        /// Asynchronously determines whether a bus schedule exists for the specified bus ID and departure time.
        /// </summary>
        /// <param name="busId">The unique identifier of the bus.</param>
        /// <param name="departureTime">The departure time to check for the bus schedule.</param>
        /// <returns>True if a matching schedule exists; otherwise, false.</returns>
        public async Task<bool> ScheduleExistsAsync(int busId, DateTime departureTime)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT COUNT(*)
                                   FROM bbs.BusSchedule
                                  WHERE BusId=@BusId
                                    AND DepartureTime=@DepartureTime";

            int count = await connection.ExecuteScalarAsync<int>(
                        sql,
                        new
                        {
                            BusId = busId,
                            DepartureTime = departureTime
                        });

            return count > 0;
        }

        /// <summary>
        /// Determines whether the specified bus has a scheduling conflict within the given time range.
        /// </summary>
        /// <param name="busId">The unique identifier of the bus.</param>
        /// <param name="departureTime">The scheduled departure time.</param>
        /// <param name="arrivalTime">The scheduled arrival time.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if a scheduling conflict
        /// exists; otherwise, false.</returns>
        public async Task<bool> HasScheduleConflictAsync(int busId, DateTime departureTime, DateTime arrivalTime)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT COUNT(*)
                                   FROM bbs.BusSchedule
                                  WHERE BusId = @BusId
                                    AND (@DepartureTime < ArrivalTime AND @ArrivalTime > DepartureTime)";

            int count = await connection.ExecuteScalarAsync<int>(
                    sql,
                    new
                    {
                        BusId = busId,
                        DepartureTime = departureTime,
                        ArrivalTime = arrivalTime
                    });

            return count > 0;
        }

        public async Task<int> DeductSeatsAsync(int scheduleId, int seatsToDeduct, IDbConnection connection, IDbTransaction transaction)
        {
            const string sql = @"UPDATE bbs.BusSchedule
                                    SET AvailableSeats = AvailableSeats - @SeatsToDeduct
                                  WHERE ScheduleId = @ScheduleId";

            return await connection.ExecuteAsync(
                sql,
                new
                {
                    ScheduleId = scheduleId,
                    SeatsToDeduct = seatsToDeduct
                },
                transaction);
        }

        public async Task<int> UpdateAvailableSeatsAsync(int scheduleId, int seatCount, IDbConnection connection, IDbTransaction transaction)
        {
            const string sql = @"UPDATE bbs.BusSchedule
                                    SET AvailableSeats = AvailableSeats + @SeatCount
                                  WHERE ScheduleId = @ScheduleId
                                    AND AvailableSeats + @SeatCount >= 0";

            return await connection.ExecuteAsync(
                sql,
                new
                {
                    ScheduleId = scheduleId,
                    SeatCount = seatCount
                },
                transaction);
        }
    }
}
