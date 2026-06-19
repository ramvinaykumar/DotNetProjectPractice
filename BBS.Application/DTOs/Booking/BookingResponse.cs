namespace BBS.Application.DTOs.Booking
{
    public class BookingResponse
    {
        public int BookingId { get; set; }

        public int PassengerId { get; set; }

        public int ScheduleId { get; set; }

        public int SeatCount { get; set; }

        public decimal TotalAmount { get; set; }

        public string BookingStatus { get; set; } = string.Empty;

        public DateTime BookingDate { get; set; }
    }
}
