using HMS.Core.Models;
using HMS.Core.Models.Dashboard;
using HMS.Core.Models.Payments;

namespace HMS.Core.Interfaces
{
    public interface IDashboardRepository
    {
        Task<(DashboardSummary Summary,
              IEnumerable<Booking> RecentBookings,
              IEnumerable<Payment> RecentPayments,
              IEnumerable<RoomStatusCount> RoomStatusBreakdown)> GetSummaryAsync();

        Task<IEnumerable<MonthlyRevenue>> GetRevenueChartAsync(int? year);

        Task<IEnumerable<OccupancyData>> GetOccupancyChartAsync();
    }
}
