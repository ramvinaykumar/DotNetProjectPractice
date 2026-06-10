namespace BBS.Domain.Entities
{
    public class BusSchedule
    {
        public int ScheduleId { get; set; }

        public int BusId { get; set; }

        public int RouteId { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal Fare { get; set; }
    }
}
