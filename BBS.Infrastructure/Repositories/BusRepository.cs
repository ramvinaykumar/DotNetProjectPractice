using BBS.Application.Interfaces.Repositories;
using BBS.Domain.Entities;
using BBS.Infrastructure.ConnectionFactory;
using Dapper;

namespace BBS.Infrastructure.Repositories
{
    public class BusRepository : IBusRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BusRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateAsync(Bus bus)
        {
            const string sql = @"

                    INSERT INTO bbs.Bus
                    (
                        BusNumber,
                        BusName,
                        TotalSeats
                    )

                    VALUES
                    (
                        @BusNumber,
                        @BusName,
                        @TotalSeats
                    )

                    SELECT CAST(
                    SCOPE_IDENTITY()
                    AS INT)";

            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QuerySingleAsync<int>(
                    sql,
                    bus);
        }

        public async Task<IEnumerable<Bus>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QueryAsync<Bus>(
                    "SELECT * FROM bbs.Bus");
        }

        public async Task<Bus?> GetByIdAsync(int busId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QueryFirstOrDefaultAsync<Bus>(
                    "SELECT * FROM bbs.Bus WHERE BusId=@BusId",
                    new { BusId = busId });
        }

        public async Task<int> UpdateAsync(Bus bus)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                @"UPDATE bbs.Bus
              SET BusNumber=@BusNumber,
                  BusName=@BusName,
                  TotalSeats=@TotalSeats
              WHERE BusId=@BusId",
                bus);
        }

        public async Task<int> DeleteAsync(int busId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                @"DELETE FROM bbs.Bus
              WHERE BusId=@BusId",
                new { BusId = busId });
        }
    }
}
