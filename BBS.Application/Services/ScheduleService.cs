using BBS.Application.Common;
using BBS.Application.DTOs.Schedule;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Services;
using BBS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BBS.Application.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        private readonly IBusRepository _busRepository;

        private readonly IRouteRepository _routeRepository;

        private readonly ILogger<ScheduleService> _logger;

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

        public async Task<int> CreateAsync(CreateScheduleRequest request)
        {
            var bus = await _busRepository.GetByIdAsync(request.BusId);

            if (bus == null)
            {
                throw new BusinessException("Bus not found");
            }

            var route = await _routeRepository.GetByIdAsync(request.RouteId);

            if (route == null)
            {
                throw new BusinessException(
                    "Route not found");
            }

            if (request.ArrivalTime <= request.DepartureTime)
            {
                throw new BusinessException("Arrival Time must be greater than Departure Time");
            }

            bool exists = await _scheduleRepository.ScheduleExistsAsync(request.BusId, request.DepartureTime);

            if (exists)
            {
                throw new BusinessException("Duplicate schedule found");
            }

            var schedule = new BusSchedule
            {
                BusId = request.BusId,
                RouteId = request.RouteId,
                DepartureTime = request.DepartureTime,
                ArrivalTime = request.ArrivalTime,
                Fare = request.Fare
            };

            return await _scheduleRepository.CreateAsync(schedule);
        }

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

        public async Task<ScheduleResponse?> GetByIdAsync(int id)
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

        public async Task UpdateAsync(int id, UpdateScheduleRequest request)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);

            if (schedule == null)
            {
                throw new BusinessException("Schedule not found");
            }

            schedule.DepartureTime = request.DepartureTime;

            schedule.ArrivalTime = request.ArrivalTime;

            schedule.Fare = request.Fare;

            await _scheduleRepository.UpdateAsync(schedule);
        }

        public async Task DeleteAsync(int id)
        {
            await _scheduleRepository.DeleteAsync(id);
        }
    }
}
