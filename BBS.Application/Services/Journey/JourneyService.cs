using BBS.Application.Common;
using BBS.Application.DTOs.Journey;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Repositories.Journey;
using BBS.Application.Interfaces.Services;
using BBS.Application.Models.Journey;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BBS.Application.Services.Journey
{
    /// <summary>
    /// Handles passenger journey search.
    /// </summary>
    public class JourneyService : IJourneyService
    {
        private readonly IJourneyRepository _journeyRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly ILogger<JourneyService> _logger;

        /// <summary>
        /// Parameterized constructor for JourneyService.
        /// </summary>
        /// <param name="journeyRepository">IJourneyRepository journeyRepository</param>
        /// <param name="routeRepository">IRouteRepository routeRepository</param>
        /// <param name="logger">ILogger<JourneyService> logger</param>
        public JourneyService(
            IJourneyRepository journeyRepository,
            IRouteRepository routeRepository,
            ILogger<JourneyService> logger)
        {
            _journeyRepository = journeyRepository;
            _routeRepository = routeRepository;
            _logger = logger;
        }

        /// <summary>
        /// Searches for available journeys based on the specified source, destination, and travel date.
        /// </summary>
        /// <param name="request">The search criteria including source, destination, and travel date.</param>
        /// <returns>A query result containing a list of search journey responses and additional metadata.</returns>
        /// <exception cref="BusinessException">Thrown when the source and destination are the same, the travel date is in the past, or no route exists
        /// between the specified source and destination.</exception>
        public async Task<QueryResult<SearchJourneyResponse>> SearchJourneysAsync(SearchJourneyRequest request)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Journey search started. Source:{Source}, Destination:{Destination}, TravelDate:{TravelDate}",
                request.Source, request.Destination, request.TravelDate);

            //---------------------------------------------
            // Business Validation
            //---------------------------------------------

            // Validate that source and destination are not the same
            if (string.Equals(request.Source, request.Destination, StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException("SAME_LOCATION", "Source and Destination cannot be the same.");
            }

            // Validate that travel date is not in the past
            if (request.TravelDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new BusinessException("PAST_DATE", "Travel date cannot be in the past.");
            }

            //---------------------------------------------
            // Route Exists
            //---------------------------------------------

            var route = await _routeRepository.GetBySourceAndDestinationAsync(request.Source, request.Destination);

            if (route == null)
            {
                throw new BusinessException("ROUTE_NOT_FOUND", "No route exists between the selected source and destination.");
            }

            //---------------------------------------------
            // Repository
            //---------------------------------------------

            var journeys = await _journeyRepository.SearchJourneysAsync(request);

            //---------------------------------------------
            // Mapping
            //---------------------------------------------

            var response = journeys.Select(MapToResponse)
                                    .ToList()
                                    .AsReadOnly();

            stopwatch.Stop();

            _logger.LogInformation("Journey search completed. Records:{Count}, Duration:{Duration} ms",
                response.Count, stopwatch.ElapsedMilliseconds);

            return new QueryResult<SearchJourneyResponse>
            {
                Items = response.Count > 0  ? response: null,
                TotalRecords = response.Count,
                QueryName = "Journey Search",
                ExecutionTimeInMilliseconds = stopwatch.ElapsedMilliseconds
            };
        }

        /// <summary>
        /// Maps a SearchJourneyModel to a SearchJourneyResponse.
        /// </summary>
        /// <param name="model">SearchJourneyModel model</param>
        /// <returns></returns>
        private static SearchJourneyResponse MapToResponse(SearchJourneyModel model)
        {
            return new SearchJourneyResponse
            {
                ScheduleId = model.ScheduleId,
                BusId = model.BusId,
                BusNumber = model.BusNumber,
                BusName = model.BusName,
                SourceCity = model.SourceCity,
                DestinationCity = model.DestinationCity,
                TravelDate = DateOnly.FromDateTime(model.TravelDate),
                DepartureTime = TimeOnly.FromDateTime(model.DepartureTime),
                ArrivalDate = DateOnly.FromDateTime(model.ArrivalTime),
                ArrivalTime = TimeOnly.FromDateTime(model.ArrivalTime),
                JourneyDuration = model.JourneyDuration,
                JourneyDurationText = $"{model.JourneyDuration / 60}h {model.JourneyDuration % 60}m",
                Fare = model.Fare,
                AvailableSeats = model.AvailableSeats
            };
        }
    }
}
