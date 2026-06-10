using Dapper;
using HMS.Core.Dtos.Request.Bookings;
using HMS.Core.Interfaces;
using HMS.Core.Models;
using HMS.Core.Models.Payments;
using HMS.Infrastructure.Data;
using System.Data.Common;

namespace HMS.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDbConnectionFactory _db;

        public BookingRepository(IDbConnectionFactory db) => _db = db;

        private DbConnection Conn() => (DbConnection)_db.CreateConnection();

        public async Task<(IEnumerable<Booking> Bookings, int TotalCount)> GetAllAsync(BookingQueryRequest q)
        {
            using var conn = Conn();
            var p = new DynamicParameters();
            p.Add("PageNumber", q.PageNumber);
            p.Add("PageSize", q.PageSize);
            p.Add("Status", q.Status);
            p.Add("CustomerId", q.CustomerId);
            p.Add("RoomId", q.RoomId);
            p.Add("FromDate", q.FromDate);
            p.Add("ToDate", q.ToDate);
            p.Add("SearchTerm", q.SearchTerm);

            using var multi = await conn.QueryMultipleAsync("hotel.usp_Booking_GetAll", p,
                commandType: System.Data.CommandType.StoredProcedure);
            var bookings = await multi.ReadAsync<Booking>();
            var total = await multi.ReadFirstAsync<int>();
            return (bookings, total);
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            using var conn = Conn();
            using var multi = await conn.QueryMultipleAsync("hotel.usp_Booking_GetById",
                new { BookingId = id },
                commandType: System.Data.CommandType.StoredProcedure);
            return await multi.ReadFirstOrDefaultAsync<Booking>();
        }

        public async Task<(Booking? Booking, IEnumerable<Payment> Payments)> GetByIdWithPaymentsAsync(int id)
        {
            using var conn = Conn();
            using var multi = await conn.QueryMultipleAsync("hotel.usp_Booking_GetById",
                new { BookingId = id },
                commandType: System.Data.CommandType.StoredProcedure);
            var booking = await multi.ReadFirstOrDefaultAsync<Booking>();
            var payments = await multi.ReadAsync<Payment>();
            return (booking, payments);
        }

        public async Task<int> CreateAsync(BookingCreateRequest r)
        {
            using var conn = Conn();
            return await conn.QueryFirstAsync<int>("hotel.usp_Booking_Create",
                new
                {
                    r.CustomerId,
                    r.RoomId,
                    r.StaffId,
                    CheckInDate = r.CheckInDate,
                    CheckOutDate = r.CheckOutDate,
                    r.Adults,
                    r.Children,
                    r.SpecialRequests,
                    r.DiscountAmount,
                    r.Source,
                    r.Notes
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(int id, BookingUpdateRequest r)
        {
            using var conn = Conn();
            return await conn.QueryFirstAsync<int>("hotel.usp_Booking_Update",
                new
                {
                    BookingId = id,
                    CheckInDate = r.CheckInDate,
                    CheckOutDate = r.CheckOutDate,
                    r.Adults,
                    r.Children,
                    r.SpecialRequests,
                    r.DiscountAmount,
                    r.Notes
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> CheckInAsync(int id, int? staffId)
        {
            using var conn = Conn();
            await conn.ExecuteAsync("hotel.usp_Booking_CheckIn",
                new { BookingId = id, StaffId = staffId },
                commandType: System.Data.CommandType.StoredProcedure);
            return 1;
        }

        public async Task<int> CheckOutAsync(int id)
        {
            using var conn = Conn();
            await conn.ExecuteAsync("hotel.usp_Booking_CheckOut",
                new { BookingId = id },
                commandType: System.Data.CommandType.StoredProcedure);
            return 1;
        }

        public async Task<int> CancelAsync(int id, string? reason)
        {
            using var conn = Conn();
            await conn.ExecuteAsync("hotel.usp_Booking_Cancel",
                new { BookingId = id, CancellationReason = reason },
                commandType: System.Data.CommandType.StoredProcedure);
            return 1;
        }
    }
}
