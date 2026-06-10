using System.ComponentModel.DataAnnotations;

namespace HMS.Core.Dtos.Request.Bookings
{
    public class BookingCreateRequest
    {
        [Required] public int CustomerId { get; set; }
        [Required] public int RoomId { get; set; }
        public int? StaffId { get; set; }
        [Required] public DateTime CheckInDate { get; set; }
        [Required] public DateTime CheckOutDate { get; set; }
        [Range(1, 10)] public int Adults { get; set; } = 1;
        [Range(0, 10)] public int Children { get; set; } = 0;
        public string? SpecialRequests { get; set; }
        [Range(0, 100000)] public decimal DiscountAmount { get; set; } = 0;
        public string Source { get; set; } = "WalkIn";
        public string? Notes { get; set; }
    }
}
