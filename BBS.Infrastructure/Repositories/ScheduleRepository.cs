using BBS.Application.Interfaces.Repositories;
using BBS.Domain.Entities;
using BBS.Infrastructure.ConnectionFactory;
using Dapper;

namespace BBS.Infrastructure.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ScheduleRepository( IDbConnectionFactory connectionFactory)
        {
            _connectionFactory =  connectionFactory;
        }

        public async Task<int>  CreateAsync(BusSchedule schedule)
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

                    SELECT CAST(
                        SCOPE_IDENTITY()
                        AS INT)";

            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QuerySingleAsync<int>(
                    sql,
                    schedule);
        }

        public async Task<IEnumerable<BusSchedule>> GetAllAsync()
        {
            using var connection =  _connectionFactory.CreateConnection();

            return await connection
                .QueryAsync<BusSchedule>(
                    "SELECT * FROM bbs.BusSchedule");
        }

        public async Task<BusSchedule?>  GetByIdAsync(int scheduleId)
        {
            using var connection =   _connectionFactory.CreateConnection();

            return await connection
                .QueryFirstOrDefaultAsync<BusSchedule>(
                    @"SELECT *
                  FROM bbs.BusSchedule
                  WHERE ScheduleId=@ScheduleId",
                    new { ScheduleId = scheduleId });
        }

        public async Task<int>  UpdateAsync(BusSchedule schedule)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                @"UPDATE bbs.BusSchedule
              SET DepartureTime=@DepartureTime,
                  ArrivalTime=@ArrivalTime,
                  Fare=@Fare
              WHERE ScheduleId=@ScheduleId",
                schedule);
        }

        public async Task<int> DeleteAsync(int scheduleId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                @"DELETE FROM bbs.BusSchedule
              WHERE ScheduleId=@ScheduleId",
                new
                {
                    ScheduleId = scheduleId
                });
        }

        public async Task<bool> ScheduleExistsAsync( int busId,  DateTime departureTime)
        {
            using var connection =  _connectionFactory.CreateConnection();

            const string sql =
                @"SELECT COUNT(*)
              FROM bbs.BusSchedule
              WHERE BusId=@BusId
              AND DepartureTime=@DepartureTime";

            int count = await connection
                    .ExecuteScalarAsync<int>(
                        sql,
                        new
                        {
                            BusId = busId,
                            DepartureTime = departureTime
                        });

            return count > 0;
        }
    }
}
