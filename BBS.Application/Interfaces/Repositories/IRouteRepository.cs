using BBS.Domain.Entities;

namespace BBS.Application.Interfaces.Repositories
{
    public interface IRouteRepository
    {
        Task<int> CreateAsync(Route route);

        Task<IEnumerable<Route>> GetAllAsync();

        Task<Route?> GetByIdAsync(int routeId);

        Task<int> UpdateAsync(Route route);

        Task<int> DeleteAsync(int routeId);
    }
}
