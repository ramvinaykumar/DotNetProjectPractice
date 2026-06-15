namespace BBS.Application.DTOs.Schedule
{
    public class UpdateScheduleRequest
    {
        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal Fare { get; set; }
    }
}
