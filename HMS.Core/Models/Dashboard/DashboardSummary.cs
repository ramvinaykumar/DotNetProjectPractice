namespace HMS.Core.Models.Dashboard
{
    public class DashboardSummary
    {
        public int TotalRooms { get; set; }
        public int AvailableRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public int MaintenanceRooms { get; set; }
        public int TodayCheckIns { get; set; }
        public int TodayCheckOuts { get; set; }
        public int PendingBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal TodayRevenue { get; set; }
        public int TotalCustomers { get; set; }
        public int VIPCustomers { get; set; }
        public decimal OccupancyRate { get; set; }
    }
}
