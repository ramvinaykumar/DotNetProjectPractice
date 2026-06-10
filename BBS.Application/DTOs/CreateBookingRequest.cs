namespace BBS.Application.DTOs
{
    public class CreateBookingRequest
    {
        public int ScheduleId { get; set; }

        public int PassengerId { get; set; }

        public int SeatNumber { get; set; }
    }
}
