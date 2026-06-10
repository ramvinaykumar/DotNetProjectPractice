namespace HMS.Core.Models.Dashboard
{
    public class OccupancyData
    {
        public DateTime Date { get; set; }
        public int OccupiedRooms { get; set; }
        public int TotalRooms { get; set; }
        public decimal OccupancyRate { get; set; }
    }
}
