namespace HMS.Core.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public int? StaffId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime? ActualCheckIn { get; set; }
        public DateTime? ActualCheckOut { get; set; }
        public int Nights { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public string BookingStatus { get; set; } = "Pending";
        public string? SpecialRequests { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string Source { get; set; } = "WalkIn";
        public string? Notes { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime BookedAt { get; set; }
        // Customer info
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public bool IsVIP { get; set; }
        // Room info
        public string RoomNumber { get; set; } = string.Empty;
        public int Floor { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public decimal RoomBasePrice { get; set; }
        // Staff
        public string? AssignedStaff { get; set; }
    }
}
