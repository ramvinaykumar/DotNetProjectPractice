namespace BBS.Application.Models.Journey
{
    /// <summary>
    /// Model returned from JourneyRepository.
    /// </summary>
    public class SearchJourneyModel
    {
        public int ScheduleId { get; set; }

        public int BusId { get; set; }

        public string BusNumber { get; set; } = string.Empty;

        public string BusName { get; set; } = string.Empty;

        public string SourceCity { get; set; } = string.Empty;

        public string DestinationCity { get; set; } = string.Empty;

        public DateTime TravelDate { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public int JourneyDuration { get; set; }

        public decimal Fare { get; set; }

        public int AvailableSeats { get; set; }
    }
}
