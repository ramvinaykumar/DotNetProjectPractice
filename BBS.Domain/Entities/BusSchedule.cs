namespace BBS.Domain.Entities
{
    /// <summary>
    /// Represents a bus schedule, including departure and arrival times, fare, and seating capacity.
    /// </summary>
    public class BusSchedule
    {
        public int ScheduleId { get; set; }

        public int BusId { get; set; }

        public int RouteId { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal Fare { get; set; }

        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }
    }
}
