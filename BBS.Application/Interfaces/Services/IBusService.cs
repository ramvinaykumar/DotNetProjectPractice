using BBS.Application.DTOs.Bus;

namespace BBS.Application.Interfaces.Services
{
    public interface IBusService
    {
        Task<BusResponse?> CreateAsync(CreateBusRequest request);

        Task<IEnumerable<BusResponse>> GetAllAsync();

        Task<BusResponse?> GetByIdAsync(int id);

        Task<BusResponse?> UpdateAsync(int id, UpdateBusRequest request);

        Task DeleteAsync(int id);
    }
}
