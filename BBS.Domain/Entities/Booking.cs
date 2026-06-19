namespace BBS.Domain.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int PassengerId { get; set; }

        public int ScheduleId { get; set; }

        public int SeatCount { get; set; }

        public decimal TotalAmount { get; set; }

        public string BookingStatus { get; set; } = string.Empty;

        public DateTime BookingDate { get; set; }

        public bool IsCancelled { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
