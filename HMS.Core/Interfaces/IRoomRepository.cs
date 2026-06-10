using HMS.Core.Dtos.Request.Rooms;
using HMS.Core.Models;

namespace HMS.Core.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync(string? status, int? roomTypeId, int? floor);

        Task<Room?> GetByIdAsync(int roomId);

        Task<IEnumerable<Room>> GetAvailableAsync(RoomAvailabilityRequest request);

        Task<int> CreateAsync(RoomCreateRequest request);

        Task<int> UpdateAsync(int roomId, RoomUpdateRequest request);

        Task<int> UpdateStatusAsync(int roomId, RoomStatusUpdateRequest request);

        Task<IEnumerable<RoomType>> GetRoomTypesAsync(bool isActive = true);

        Task<int> UpsertRoomTypeAsync(RoomTypeUpsertRequest request);
    }
}
