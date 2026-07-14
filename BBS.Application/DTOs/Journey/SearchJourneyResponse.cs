namespace BBS.Application.DTOs.Journey
{
    /// <summary>
    /// Represents a journey search result.
    /// </summary>
    public class SearchJourneyResponse
    {
        public int ScheduleId { get; set; }

        public int BusId { get; set; }

        public string BusNumber { get; set; } = string.Empty;

        public string BusName { get; set; } = string.Empty;

        public string SourceCity { get; set; } = string.Empty;

        public string DestinationCity { get; set; } = string.Empty;

        public DateOnly TravelDate { get; set; }

        public TimeOnly DepartureTime { get; set; }

        public DateOnly ArrivalDate { get; set; }

        public TimeOnly ArrivalTime { get; set; }

        /// <summary>
        /// Total journey duration.
        /// </summary>
        public int JourneyDuration { get; set; }

        /// <summary>
        /// Gets or sets the duration of the journey as a formatted string.
        /// </summary>
        public string JourneyDurationText { get; set; } = string.Empty;

        public decimal Fare { get; set; }

        public int AvailableSeats { get; set; }
    }
}
