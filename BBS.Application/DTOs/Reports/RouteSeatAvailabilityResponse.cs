namespace BBS.Application.DTOs.Reports
{
    /// <summary>
    /// Represents route-wise seat availability information for a scheduled bus.
    /// Used by Admin reporting screens.
    /// </summary>
    public class RouteSeatAvailabilityResponse
    {
        /// <summary>
        /// Schedule Id
        /// </summary>
        public int ScheduleId { get; set; }

        /// <summary>
        /// Bus Id
        /// </summary>
        public int BusId { get; set; }

        /// <summary>
        /// Bus Registration Number
        /// </summary>
        public string BusNumber { get; set; } = string.Empty;

        /// <summary>
        /// Bus Name
        /// </summary>
        public string BusName { get; set; } = string.Empty;

        /// <summary>
        /// Route Id
        /// </summary>
        public int RouteId { get; set; }

        /// <summary>
        /// Route Source
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Route Destination
        /// </summary>
        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// Departure Time
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// Arrival Time
        /// </summary>
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// Fare Per Seat
        /// </summary>
        public decimal Fare { get; set; }

        /// <summary>
        /// Total Seats
        /// </summary>
        public int TotalSeats { get; set; }

        /// <summary>
        /// Booked Seats
        /// </summary>
        public int BookedSeats { get; set; }

        /// <summary>
        /// Available Seats
        /// </summary>
        public int AvailableSeats { get; set; }

        /// <summary>
        /// Occupancy Percentage
        /// </summary>
        public decimal OccupancyPercentage { get; set; }
    }
}
