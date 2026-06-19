using BBS.Application.Common;
using BBS.Application.DTOs.Schedule;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Services;
using BBS.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BBS.Application.Services
{
    /// <summary>
    /// Provides operations for managing bus schedules, including creation, retrieval, update, and deletion.
    /// </summary>
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IBusRepository _busRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly ILogger<ScheduleService> _logger;

        /// <summary>
        /// Parameterized constructor that initializes the ScheduleService with the required repositories and logger.
        /// </summary>
        /// <param name="scheduleRepository">IScheduleRepository scheduleRepository</param>
        /// <param name="busRepository">IBusRepository busRepository</param>
        /// <param name="routeRepository">IRouteRepository routeRepository</param>
        /// <param name="logger">ILogger<ScheduleService> logger</param>
        public ScheduleService(
            IScheduleRepository scheduleRepository,
            IBusRepository busRepository,
            IRouteRepository routeRepository,
            ILogger<ScheduleService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _busRepository = busRepository;
            _routeRepository = routeRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new bus schedule asynchronously.
        /// </summary>
        /// <param name="request">The details of the schedule to create.</param>
        /// <returns>The identifier of the newly created schedule.</returns>
        /// <exception cref="BusinessException">Thrown when the bus or route is not found, when the arrival time is not greater than the departure time, or
        /// when a duplicate schedule is detected.</exception>
        public async Task<ScheduleResponse> CreateAsync(CreateScheduleRequest request)
        {
            _logger.LogInformation("Creating schedule for BusId: {BusId} and RouteId: {RouteId}", request.BusId, request.RouteId);
            var bus = await _busRepository.GetByIdAsync(request.BusId);

            if (bus == null)
            {
                throw new BusinessException("Bus not found");
            }

            var route = await _routeRepository.GetByIdAsync(request.RouteId);
            if (route == null)
            {
                throw new BusinessException("Route not found");
            }

            var routeJson = JsonSerializer.Serialize(route);

            _logger.LogInformation("Retrieved bus and route information for BusId: {BusId}, RouteId: {RouteId}. Route Data: {RouteData}",
                request.BusId, request.RouteId, routeJson);

            if (request.ArrivalTime <= request.DepartureTime)
            {
                throw new BusinessException("Arrival Time must be greater than Departure Time");
            }

            var hasConflict = await _scheduleRepository.HasScheduleConflictAsync(request.BusId, request.DepartureTime, request.ArrivalTime);

            if (hasConflict)
            {
                throw new BusinessException("Bus already has another schedule during this time.");
            }

            _logger.LogInformation("No schedule conflict detected for BusId: {BusId} between DepartureTime: {DepartureTime} and ArrivalTime: {ArrivalTime}",
                request.BusId, request.DepartureTime, request.ArrivalTime);

            bool exists = await _scheduleRepository.ScheduleExistsAsync(request.BusId, request.DepartureTime);

            _logger.LogInformation("Checking for existing schedule for BusId: {BusId} at DepartureTime: {DepartureTime}. Exists: {Exists}",
                request.BusId, request.DepartureTime, exists);

            if (exists)
            {
                throw new BusinessException("Schedule already exists.");
            }

            var schedule = new BusSchedule
            {
                BusId = request.BusId,
                RouteId = request.RouteId,
                DepartureTime = request.DepartureTime,
                ArrivalTime = request.ArrivalTime,
                Fare = request.Fare
            };

            _logger.LogInformation("Creating new schedule: {@Schedule}", schedule);

            var id = await _scheduleRepository.CreateAsync(schedule);
            return await GetScheduleDataByIdAsync(id);
        }

        /// <summary>
        /// Asynchronously retrieves all schedule entries.
        /// </summary>
        /// <returns>A collection of ScheduleResponse objects representing the schedules.</returns>
        public async Task<IEnumerable<ScheduleResponse>> GetAllAsync()
        {
            var result = await _scheduleRepository.GetAllAsync();

            return result.Select(x =>
                new ScheduleResponse
                {
                    ScheduleId = x.ScheduleId,
                    BusId = x.BusId,
                    RouteId = x.RouteId,
                    DepartureTime = x.DepartureTime,
                    ArrivalTime = x.ArrivalTime,
                    Fare = x.Fare
                });
        }

        /// <summary>
        /// Retrieves a schedule by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the schedule.</param>
        /// <returns>A ScheduleResponse containing the schedule details, or null if not found.</returns>
        /// <exception cref="BusinessException">Thrown when no schedule is found for the specified identifier.</exception>
        public async Task<ScheduleResponse?> GetByIdAsync(int id)
        {
            return await GetScheduleDataByIdAsync(id);
        }

        /// <summary>
        /// Updates an existing schedule with the specified values.
        /// </summary>
        /// <param name="id">The unique identifier of the schedule to update.</param>
        /// <param name="request">The updated schedule information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="BusinessException">Thrown when a schedule with the specified identifier does not exist.</exception>
        public async Task<ScheduleResponse> UpdateAsync(int id, UpdateScheduleRequest request)
        {
            _logger.LogInformation("Updating schedule with ID: {ScheduleId}. New DepartureTime: {DepartureTime}, New ArrivalTime: {ArrivalTime}, New Fare: {Fare}",
                id, request.DepartureTime, request.ArrivalTime, request.Fare);

            var schedule = await _scheduleRepository.GetByIdAsync(id);

            if (schedule == null)
            {
                throw new BusinessException("Schedule not found");
            }

            _logger.LogInformation("Current schedule data: {@Schedule}", schedule);

            schedule.DepartureTime = request.DepartureTime;
            schedule.ArrivalTime = request.ArrivalTime;
            schedule.Fare = request.Fare;

            await _scheduleRepository.UpdateAsync(schedule);

            _logger.LogInformation("Schedule with ID: {ScheduleId} updated successfully. Schedule details are {@Schedule}", id, schedule);

            return await GetScheduleDataByIdAsync(id);
        }

        /// <summary>
        /// Asynchronously deletes a schedule entry by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the schedule entry to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task DeleteAsync(int id)
        {
            await _scheduleRepository.DeleteAsync(id);
        }

        private async Task<ScheduleResponse> GetScheduleDataByIdAsync(int id)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null)
            {
                throw new BusinessException("Schedule not found");
            }
            return new ScheduleResponse
            {
                ScheduleId = schedule.ScheduleId,
                BusId = schedule.BusId,
                RouteId = schedule.RouteId,
                DepartureTime = schedule.DepartureTime,
                ArrivalTime = schedule.ArrivalTime,
                Fare = schedule.Fare
            };
        }
    }
}
