using BBS.Application.Interfaces.Infrastructure;
using BBS.Application.Interfaces.Repositories;
using BBS.Domain.Entities;
using Dapper;

namespace BBS.Infrastructure.Repositories
{
    /// <summary>
    /// Repository responsible for managing routes in the database.
    /// </summary>
    public class RouteRepository : IRouteRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RouteRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Inserts a new route into the database asynchronously and returns the generated identifier.
        /// </summary>
        /// <param name="route">The route entity containing the details to insert.</param>
        /// <returns>The identifier of the newly created route.</returns>
        public async Task<int> CreateAsync(Route route)
        {
            const string sql = @"INSERT INTO bbs.Route
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

                                SELECT CAST(SCOPE_IDENTITY() AS INT)";

            using var connection = _connectionFactory.CreateConnection();

            return await connection.QuerySingleAsync<int>(sql, route);
        }

        /// <summary>
        /// Gets all routes from the database asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Route>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<Route>("SELECT RouteId, SourceCity, DestinationCity, DistanceKM, IsActive FROM bbs.Route");
        }

        /// <summary>
        /// Get route by its identifier asynchronously.
        /// </summary>
        /// <param name="routeId">int routeId</param>
        /// <returns></returns>
        public async Task<Route?> GetByIdAsync(int routeId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QueryFirstOrDefaultAsync<Route>(
                    @"SELECT RouteId, SourceCity, DestinationCity, DistanceKM, IsActive
                        FROM bbs.Route
                       WHERE RouteId=@RouteId",
                        new { RouteId = routeId });
        }

        /// <summary>
        /// Updates an existing route in the database asynchronously.
        /// </summary>
        /// <param name="route">Route route</param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a route from the database asynchronously based on its identifier.
        /// </summary>
        /// <param name="routeId">int routeId</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int routeId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(
                @"DELETE FROM bbs.Route
                   WHERE RouteId=@RouteId",
                new { RouteId = routeId });
        }

        /// <summary>
        /// Gets a route by its source and destination cities asynchronously.
        /// </summary>
        /// <param name="source">string source</param>
        /// <param name="destination">string destination</param>
        /// <returns></returns>
        public async Task<Route?> GetBySourceAndDestinationAsync(string source, string destination)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Route>
                (@"SELECT RouteId, SourceCity, DestinationCity, DistanceKM FROM bbs.Route
                    WHERE SourceCity = @SourceCity AND DestinationCity = @DestinationCity",
                    new
                    {
                        SourceCity = source,
                        DestinationCity = destination
                    });
        }
    }
}
