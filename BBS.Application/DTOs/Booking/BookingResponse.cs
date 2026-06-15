namespace BBS.Application.DTOs.Booking
{
    public class BookingResponse
    {
        public int BookingId { get; set; }

        public int SeatNumber { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
