using BBS.Application.DTOs.Bus;

namespace BBS.Application.Interfaces.Services
{
    public interface IBusService
    {
        Task<int> CreateAsync(CreateBusRequest request);

        Task<IEnumerable<BusResponse>> GetAllAsync();

        Task<BusResponse?> GetByIdAsync(int id);

        Task UpdateAsync(int id, UpdateBusRequest request);

        Task DeleteAsync(int id);
    }
}
