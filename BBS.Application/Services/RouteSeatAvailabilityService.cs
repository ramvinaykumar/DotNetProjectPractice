using BBS.Application.Common;
using BBS.Application.DTOs.Reports;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Repositories.Reports;
using BBS.Application.Interfaces.Services;
using BBS.Application.Models.Reports;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BBS.Application.Services
{
    /// <summary>
    /// Service responsible for reporting operations.
    /// </summary>
    public class RouteSeatAvailabilityService : IRouteSeatAvailabilityService
    {
        private readonly IRouteSeatAvailabilityRepository _reportingRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly ILogger<RouteSeatAvailabilityService> _logger;

        /// <summary>
        /// Parameterized constructor for ReportingService.
        /// </summary>
        /// <param name="reportingRepository">IReportingRepository reportingRepository</param>
        /// <param name="routeRepository">IRouteRepository routeRepository</param>
        /// <param name="logger">ILogger<ReportingService> logger</param>
        public RouteSeatAvailabilityService(
            IRouteSeatAvailabilityRepository reportingRepository,
            IRouteRepository routeRepository,
            ILogger<RouteSeatAvailabilityService> logger)
        {
            _reportingRepository = reportingRepository;
            _routeRepository = routeRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves seat availability information for the specified route.
        /// </summary>
        /// <param name="routeId">The unique identifier of the route.</param>
        /// <returns>A QueryResult containing a read-only list of seat availability responses and related metadata.</returns>
        /// <exception cref="BusinessException">Thrown when the routeId is invalid or the route does not exist.</exception>
        public async Task<QueryResult<RouteSeatAvailabilityResponse>> GetRouteSeatAvailabilityAsync(int routeId)
        {
            _logger.LogInformation("Route seat availability requested. RouteId:{RouteId}", routeId);
            var stopwatch = Stopwatch.StartNew();

            // Validate Route Id
            if (routeId <= 0)
            {
                throw new BusinessException("Invalid Route Id.");
            }

            // Route Exists
            var route = await _routeRepository.GetByIdAsync(routeId);
            if (route == null)
            {
                throw new BusinessException("Route not found.");
            }

            //----------------------------------------------------
            // Route Active
            //----------------------------------------------------

            // Uncomment if Route has IsActive

            /*
            if(!route.IsActive)
            {
                throw new BusinessException( "Route is inactive.");
            }
            */


            // Fetch Data
            var report = await _reportingRepository.GetRouteSeatAvailabilityAsync(routeId);

            stopwatch.Stop();

            //----------------------------------------------------
            // Mapping the report model to response DTO
            //----------------------------------------------------
            var response = report.Select(MapToResponse).ToList().AsReadOnly();

            _logger.LogInformation( "Retrieved {ScheduleCount} schedules for RouteId:{RouteId}. TotalBookedSeats:{BookedSeats}",
                        response.Count,
                        routeId,
                        response.Sum(x => x.BookedSeats));

            return new QueryResult<RouteSeatAvailabilityResponse>
            {
                Items = response.Count > 0 ? response : null,
                TotalRecords = response.Count,
                QueryName = "Route Seat Availability",
                ExecutionTimeInMilliseconds =  stopwatch.ElapsedMilliseconds,
                GeneratedOnUtc = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a SeatAvailabilityResponse populated with data from the specified SeatAvailabilityReportModel.
        /// </summary>
        /// <param name="model">The report model containing seat availability information.</param>
        /// <returns>A SeatAvailabilityResponse with values mapped from the model.</returns>
        private static RouteSeatAvailabilityResponse MapToResponse(RouteSeatAvailabilityReportModel model)
        {
            return new RouteSeatAvailabilityResponse
            {
                ScheduleId = model.ScheduleId,
                BusId = model.BusId,
                BusNumber = model.BusNumber,
                BusName = model.BusName,
                RouteId = model.RouteId,
                Source = model.SourceCity,
                Destination = model.DestinationCity,
                DepartureTime = model.DepartureTime,
                ArrivalTime = model.ArrivalTime,
                Fare = model.Fare,
                TotalSeats = model.TotalSeats,
                BookedSeats = model.BookedSeats,
                AvailableSeats = model.AvailableSeats,
                OccupancyPercentage = model.OccupancyPercentage
            };
        }
    }
}
