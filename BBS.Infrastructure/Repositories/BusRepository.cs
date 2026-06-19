using BBS.Application.Interfaces.Infrastructure;
using BBS.Application.Interfaces.Repositories;
using BBS.Domain.Entities;
using Dapper;

namespace BBS.Infrastructure.Repositories
{
    /// <summary>
    /// Provides data access methods for managing bus entities in the database.
    /// </summary>
    public class BusRepository : IBusRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the BusRepository class.
        /// </summary>
        /// <param name="connectionFactory">The factory used to create database connections.</param>
        public BusRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Creates a new bus record in the database asynchronously.
        /// </summary>
        /// <param name="bus">Bus object</param>
        /// <returns>Return busId or zero</returns>
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

                    SELECT CAST(SCOPE_IDENTITY() AS INT)";

            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QuerySingleAsync<int>(
                    sql, bus);
        }

        /// <summary>
        /// Retrieves all bus records from the database asynchronously.
        /// </summary>
        /// <returns>all bus records from the database</returns>
        public async Task<IEnumerable<Bus>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Bus>("SELECT * FROM bbs.Bus");
        }

        /// <summary>
        /// Asynchronously retrieves a bus by its unique identifier.
        /// </summary>
        /// <param name="busId">The unique identifier of the bus.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the bus if found; otherwise,
        /// null.</returns>
        public async Task<Bus?> GetByIdAsync(int busId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QueryFirstOrDefaultAsync<Bus>(
                    "SELECT * FROM bbs.Bus WHERE BusId=@BusId",
                    new { BusId = busId });
        }

        /// <summary>
        /// Asynchronously updates the details of a bus in the database.
        /// </summary>
        /// <param name="bus">The bus entity containing updated information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
        public async Task<int> UpdateAsync(Bus bus)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                 @"UPDATE bbs.Bus
                      SET BusNumber=@BusNumber,
                          BusName=@BusName,
                          TotalSeats=@TotalSeats
                    WHERE BusId=@BusId", bus);
        }

        /// <summary>
        /// Asynchronously deletes a bus record with the specified identifier.
        /// </summary>
        /// <param name="busId">The unique identifier of the bus to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
        public async Task<int> DeleteAsync(int busId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(@"DELETE FROM bbs.Bus WHERE BusId=@BusId", new { BusId = busId });
        }
    }
}
