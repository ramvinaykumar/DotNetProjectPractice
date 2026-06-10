using HMS.Core.Models;
using HMS.Core.Models.Dashboard;
using HMS.Core.Models.Payments;

namespace HMS.Core.Dtos.Response
{
    public class DashboardResponse
    {
        public DashboardSummary Summary { get; set; } = null!;
        public IEnumerable<Booking> RecentBookings { get; set; } = [];
        public IEnumerable<Payment> RecentPayments { get; set; } = [];
        public IEnumerable<RoomStatusCount> RoomStatusBreakdown { get; set; } = [];
    }
}
