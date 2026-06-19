using BBS.Application.Interfaces.Infrastructure;
using BBS.Application.Interfaces.Repositories;
using BBS.Domain.Entities;
using Dapper;

namespace BBS.Infrastructure.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RouteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateAsync(Route route)
        {
            const string sql = @"

                    INSERT INTO bbs.Route
                    (
                        SourceCity,
                        DestinationCity,
                        DistanceKM
                    )
                    VALUES
                    (
                        @SourceCity,
                        @DestinationCity,
                        @DistanceKM
                    )

                    SELECT CAST(
                        SCOPE_IDENTITY()
                        AS INT)";

            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QuerySingleAsync<int>(
                    sql,
                    route);
        }

        public async Task<IEnumerable<Route>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Route>("SELECT * FROM bbs.Route");
        }

        public async Task<Route?> GetByIdAsync(int routeId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QueryFirstOrDefaultAsync<Route>(
                    @"SELECT *
                  FROM bbs.Route
                  WHERE RouteId=@RouteId",
                    new { RouteId = routeId });
        }

        public async Task<int> UpdateAsync(Route route)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                @"UPDATE bbs.Route
              SET SourceCity=@SourceCity,
                  DestinationCity=@DestinationCity,
                  DistanceKM=@DistanceKM
              WHERE RouteId=@RouteId",
                route);
        }

        public async Task<int> DeleteAsync(int routeId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                @"DELETE FROM bbs.Route
              WHERE RouteId=@RouteId",
                new { RouteId = routeId });
        }
    }
}
