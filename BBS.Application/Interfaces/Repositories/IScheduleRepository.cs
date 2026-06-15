using BBS.Domain.Entities;

namespace BBS.Application.Interfaces.Repositories
{
    public interface IScheduleRepository
    {
        Task<int> CreateAsync(BusSchedule schedule);

        Task<IEnumerable<BusSchedule>> GetAllAsync();

        Task<BusSchedule?> GetByIdAsync(int scheduleId);

        Task<int> UpdateAsync(BusSchedule schedule);

        Task<int> DeleteAsync(int scheduleId);

        Task<bool> ScheduleExistsAsync(int busId, DateTime departureTime);
    }
}
