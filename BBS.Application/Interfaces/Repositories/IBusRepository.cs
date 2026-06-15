using BBS.Domain.Entities;

namespace BBS.Application.Interfaces.Repositories
{
    public interface IBusRepository
    {
        Task<int> CreateAsync(Bus bus);

        Task<IEnumerable<Bus>> GetAllAsync();

        Task<Bus?> GetByIdAsync(int busId);

        Task<int> UpdateAsync(Bus bus);

        Task<int> DeleteAsync(int busId);
    }
}
