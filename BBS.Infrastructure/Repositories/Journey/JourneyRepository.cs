using BBS.Application.DTOs.Journey;
using BBS.Application.Interfaces.Infrastructure;
using BBS.Application.Interfaces.Repositories.Journey;
using BBS.Application.Models.Journey;
using Dapper;

namespace BBS.Infrastructure.Repositories.Journey
{
    /// <summary>
    /// Repository responsible for passenger journey searches.
    /// </summary>
    public class JourneyRepository : IJourneyRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public JourneyRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Retrieves available bus journeys matching the specified search criteria.
        /// </summary>
        /// <param name="request">The search criteria including source city, destination city, and travel date.</param>
        /// <returns>A read-only list of journey models that match the search criteria.</returns>
        public async Task<IReadOnlyList<SearchJourneyModel>> SearchJourneysAsync(SearchJourneyRequest request)
        {
            using var connection = _connectionFactory.CreateConnection();

            var travelDate = request.TravelDate.ToDateTime(TimeOnly.MinValue);

            const string sql = @"SELECT bs.ScheduleId, b.BusId, b.BusNumber, b.BusName, r.SourceCity, r.DestinationCity, CAST(bs.DepartureTime AS DATE) AS TravelDate
                                        ,bs.DepartureTime, bs.ArrivalTime, DATEDIFF (MINUTE, bs.DepartureTime, bs.ArrivalTime ) AS JourneyDuration
                                        ,bs.Fare, bs.AvailableSeats
                                   FROM bbs.BusSchedule bs
                                  INNER JOIN bbs.Bus b ON bs.BusId = b.BusId
                                  INNER JOIN bbs.Route r ON bs.RouteId = r.RouteId
                                  WHERE r.SourceCity=@Source 
                                    AND r.DestinationCity=@Destination
                                    AND bs.DepartureTime>=@TravelDate
                                    AND bs.DepartureTime < DATEADD(DAY,1,@TravelDate)
                                    AND bs.AvailableSeats > 0
                                    AND b.IsActive=1
                                    AND r.IsActive=1
                                  ORDER BY bs.DepartureTime;";

            var result = await connection.QueryAsync<SearchJourneyModel>(
                    sql,
                    new
                    {
                        request.Source,
                        request.Destination,
                        TravelDate = travelDate
                    });

            return result.AsList();
        }
    }
}
