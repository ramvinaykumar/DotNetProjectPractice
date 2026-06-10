using Dapper;
using HMS.Core.Dtos.Request.Rooms;
using HMS.Core.Interfaces;
using HMS.Core.Models;
using HMS.Infrastructure.Data;
using System.Data.Common;

namespace HMS.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IDbConnectionFactory _db;
        public RoomRepository(IDbConnectionFactory db) => _db = db;

        private DbConnection Conn() => (DbConnection)_db.CreateConnection();

        public async Task<IEnumerable<Room>> GetAllAsync(string? status, int? roomTypeId, int? floor)
        {
            using var conn = Conn();
            return await conn.QueryAsync<Room>("hotel.usp_Room_GetAll",
                new { Status = status, RoomTypeId = roomTypeId, Floor = floor, IsActive = (bool?)true },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Room?> GetByIdAsync(int roomId)
        {
            using var conn = Conn();
            return await conn.QueryFirstOrDefaultAsync<Room>("hotel.usp_Room_GetById",
                new { RoomId = roomId },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Room>> GetAvailableAsync(RoomAvailabilityRequest r)
        {
            using var conn = Conn();
            return await conn.QueryAsync<Room>("hotel.usp_Room_GetAvailable",
                new
                {
                    CheckIn = r.CheckInDate,
                    CheckOut = r.CheckOutDate,
                    RoomTypeId = r.RoomTypeId,
                    Adults = r.Adults,
                    Children = r.Children
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> CreateAsync(RoomCreateRequest r)
        {
            using var conn = Conn();
            return await conn.QueryFirstAsync<int>("hotel.usp_Room_Create",
                new { r.RoomTypeId, r.RoomNumber, r.Floor, r.Description },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(int roomId, RoomUpdateRequest r)
        {
            using var conn = Conn();
            return await conn.QueryFirstAsync<int>("hotel.usp_Room_Update",
                new
                {
                    RoomId = roomId,
                    r.RoomTypeId,
                    r.RoomNumber,
                    r.Floor,
                    r.Status,
                    r.Description,
                    r.IsActive
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> UpdateStatusAsync(int roomId, RoomStatusUpdateRequest r)
        {
            using var conn = Conn();
            return await conn.QueryFirstAsync<int>("hotel.usp_Room_UpdateStatus",
                new { RoomId = roomId, r.Status, r.LastCleaned },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<RoomType>> GetRoomTypesAsync(bool isActive = true)
        {
            using var conn = Conn();
            return await conn.QueryAsync<RoomType>("hotel.usp_RoomType_GetAll",
                new { IsActive = isActive },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> UpsertRoomTypeAsync(RoomTypeUpsertRequest r)
        {
            using var conn = Conn();
            return await conn.QueryFirstAsync<int>("hotel.usp_RoomType_Upsert",
                new
                {
                    r.RoomTypeId,
                    r.TypeName,
                    r.Description,
                    r.BasePrice,
                    r.MaxOccupancy,
                    r.AmenitiesList,
                    r.ImageUrl,
                    r.IsActive
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
