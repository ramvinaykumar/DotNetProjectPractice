using BBS.Application.DTOs.Route;

namespace BBS.Application.Interfaces.Services
{
    public interface IRouteService
    {
        Task<RouteResponse> CreateAsync(CreateRouteRequest request);

        Task<IEnumerable<RouteResponse>> GetAllAsync();

        Task<RouteResponse?> GetByIdAsync(int id);

        Task<RouteResponse> UpdateAsync(int id, UpdateRouteRequest request);

        Task DeleteAsync(int id);
    }
}
