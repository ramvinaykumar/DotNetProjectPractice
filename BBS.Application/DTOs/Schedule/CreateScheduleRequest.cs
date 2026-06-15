namespace BBS.Application.DTOs.Schedule
{
    public class CreateScheduleRequest
    {
        public int BusId { get; set; }

        public int RouteId { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal Fare { get; set; }
    }
}
