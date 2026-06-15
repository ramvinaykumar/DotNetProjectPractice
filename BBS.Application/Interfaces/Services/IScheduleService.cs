using BBS.Application.DTOs.Schedule;

namespace BBS.Application.Interfaces.Services
{
    public interface IScheduleService
    {
        Task<int> CreateAsync(CreateScheduleRequest request);

        Task<IEnumerable<ScheduleResponse>> GetAllAsync();

        Task<ScheduleResponse?> GetByIdAsync(int id);

        Task UpdateAsync(int id, UpdateScheduleRequest request);

        Task DeleteAsync(int id);
    }
}
