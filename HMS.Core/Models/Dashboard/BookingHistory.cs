namespace HMS.Core.Models.Dashboard
{
    public class BookingHistory
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Nights { get; set; }
        public decimal FinalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal PaidAmount { get; set; }
    }
}
