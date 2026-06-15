using BBS.Application.Common;
using BBS.Application.DTOs.Route;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Services;
using BBS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BBS.Application.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _repository;

        private readonly ILogger<RouteService> _logger;

        public RouteService(IRouteRepository repository, ILogger<RouteService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RouteResponse> CreateAsync(CreateRouteRequest request)
        {
            var route = new Route
            {
                SourceCity = request.SourceCity,
                DestinationCity = request.DestinationCity,
                DistanceKM = request.DistanceKM
            };

            var routeId = await _repository.CreateAsync(route);
            var createdRoute = await GetDataById(routeId);

            return new RouteResponse
            {
                RouteId = createdRoute.RouteId,
                SourceCity = createdRoute.SourceCity,
                DestinationCity = createdRoute.DestinationCity,
                DistanceKM = createdRoute.DistanceKM
            };
        }

        public async Task<IEnumerable<RouteResponse>> GetAllAsync()
        {
            var routes = await _repository.GetAllAsync();

            return routes.Select(x =>
                new RouteResponse
                {
                    RouteId = x.RouteId,
                    SourceCity = x.SourceCity,
                    DestinationCity =  x.DestinationCity,
                    DistanceKM = x.DistanceKM
                });
        }

        public async Task<RouteResponse?> GetByIdAsync(int id)
        {
            return await GetDataById(id);
        }

        public async Task<RouteResponse> UpdateAsync(int id, UpdateRouteRequest request)
        {
            var route = await _repository.GetByIdAsync(id);

            if (route == null)
            {
                throw new BusinessException("Route not found");
            }

            route.SourceCity = request.SourceCity;
            route.DestinationCity = request.DestinationCity;
            route.DistanceKM = request.DistanceKM;

            await _repository.UpdateAsync(route);

            return await GetDataById(id);
        }

        public async Task DeleteAsync(int id)
        {
            var route = await _repository.GetByIdAsync(id);

            if (route == null)
            {
                throw new BusinessException("Route not found");
            }

            await _repository.DeleteAsync(id);
        }

        private async Task<RouteResponse> GetDataById(int id)
        {
            var route = await _repository.GetByIdAsync(id);

            if (route == null)
            {
                throw new BusinessException("Route not found");
            }

            return new RouteResponse
            {
                RouteId = route.RouteId,
                SourceCity = route.SourceCity,
                DestinationCity = route.DestinationCity,
                DistanceKM = route.DistanceKM
            };
        }
    }
}
