namespace BBS.Application.Models.Reports
{
    /// <summary>
    /// Represents the seat availability report model returned by the ReportingRepository.
    /// This is an internal read model and should not be exposed directly to the API.
    /// </summary>
    public class RouteSeatAvailabilityReportModel
    {
        /// <summary>
        /// Schedule Id.
        /// </summary>
        public int ScheduleId { get; set; }

        /// <summary>
        /// Bus Id.
        /// </summary>
        public int BusId { get; set; }

        /// <summary>
        /// Bus registration number.
        /// </summary>
        public string BusNumber { get; set; } = string.Empty;

        /// <summary>
        /// Bus display name.
        /// </summary>
        public string BusName { get; set; } = string.Empty;

        /// <summary>
        /// Route Id.
        /// </summary>
        public int RouteId { get; set; }

        /// <summary>
        /// Route source.
        /// </summary>
        public string SourceCity { get; set; } = string.Empty;

        /// <summary>
        /// Route destination.
        /// </summary>
        public string DestinationCity { get; set; } = string.Empty;

        /// <summary>
        /// Departure date and time.
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// Arrival date and time.
        /// </summary>
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// Fare per passenger.
        /// </summary>
        public decimal Fare { get; set; }

        /// <summary>
        /// Total seats in the bus.
        /// </summary>
        public int TotalSeats { get; set; }

        /// <summary>
        /// Number of booked seats.
        /// </summary>
        public int BookedSeats { get; set; }

        /// <summary>
        /// Number of available seats.
        /// </summary>
        public int AvailableSeats { get; set; }

        /// <summary>
        /// Occupancy percentage.
        /// </summary>
        public decimal OccupancyPercentage { get; set; }
    }
}
