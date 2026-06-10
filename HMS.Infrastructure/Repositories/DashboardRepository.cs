using Dapper;
using HMS.Core.Interfaces;
using HMS.Core.Models;
using HMS.Core.Models.Dashboard;
using HMS.Core.Models.Payments;
using HMS.Infrastructure.Data;
using System.Data.Common;

namespace HMS.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IDbConnectionFactory _db;

        public DashboardRepository(IDbConnectionFactory db) => _db = db;

        private DbConnection Conn() => (DbConnection)_db.CreateConnection();

        public async Task<(DashboardSummary Summary,
                           IEnumerable<Booking> RecentBookings,
                           IEnumerable<Payment> RecentPayments,
                           IEnumerable<RoomStatusCount> RoomStatusBreakdown)> GetSummaryAsync()
        {
            using var conn = Conn();
            using var multi = await conn.QueryMultipleAsync("hotel.usp_Dashboard_GetSummary",
                commandType: System.Data.CommandType.StoredProcedure);

            var summary = await multi.ReadFirstAsync<DashboardSummary>();
            var bookings = await multi.ReadAsync<Booking>();
            var payments = await multi.ReadAsync<Payment>();
            var roomStatus = await multi.ReadAsync<RoomStatusCount>();
            return (summary, bookings, payments, roomStatus);
        }

        public async Task<IEnumerable<MonthlyRevenue>> GetRevenueChartAsync(int? year)
        {
            using var conn = Conn();
            return await conn.QueryAsync<MonthlyRevenue>("hotel.usp_Dashboard_GetRevenueChart",
                new { Year = year },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<OccupancyData>> GetOccupancyChartAsync()
        {
            using var conn = Conn();
            return await conn.QueryAsync<OccupancyData>("hotel.usp_Dashboard_GetOccupancyChart",
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
