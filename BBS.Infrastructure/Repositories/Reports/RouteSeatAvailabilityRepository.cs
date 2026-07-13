using BBS.Application.Interfaces.Infrastructure;
using BBS.Application.Interfaces.Repositories.Reports;
using BBS.Application.Models.Reports;
using Dapper;

namespace BBS.Infrastructure.Repositories.Reports
{
    /// <summary>
    /// Provides access to route seat availability data using a database connection factory.
    /// </summary>
    /// <remarks>Implements methods to retrieve seat availability reports for specific routes from the
    /// database.</remarks>
    public class RouteSeatAvailabilityRepository : IRouteSeatAvailabilityRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Parameterized constructor for RouteSeatAvailabilityRepository.
        /// </summary>
        /// <param name="connectionFactory">IDbConnectionFactory connectionFactory</param>
        public RouteSeatAvailabilityRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Asynchronously retrieves seat availability reports for a specified route.
        /// </summary>
        /// <param name="routeId">The route identifier to query seat availability for.</param>
        /// <returns>A read-only list of seat availability report models for the given route.</returns>
        public async Task<IReadOnlyList<RouteSeatAvailabilityReportModel>> GetRouteSeatAvailabilityAsync(int routeId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT bs.ScheduleId, b.BusId, b.BusNumber, b.BusName, r.RouteId, r.SourceCity, r.DestinationCity
                                        ,bs.DepartureTime, bs.ArrivalTime, bs.Fare, NULLIF(bs.TotalSeats,0) AS TotalSeats, (bs.TotalSeats-bs.AvailableSeats) AS BookedSeats
                                        ,bs.AvailableSeats, CAST(((bs.TotalSeats-bs.AvailableSeats) * 100.0) / bs.TotalSeats AS DECIMAL(5,2)) AS OccupancyPercentage
                                   FROM bbs.BusSchedule bs
                                  INNER JOIN bbs.Bus b ON bs.BusId=b.BusId
                                  INNER JOIN bbs.Route r ON bs.RouteId=r.RouteId
                                  WHERE bs.RouteId=@RouteId
                                  ORDER BY bs.DepartureTime";

            var result =  await connection.QueryAsync<RouteSeatAvailabilityReportModel>(sql,
                        new
                        {
                            RouteId = routeId
                        });

            return result.ToList();
        }
    }
}
