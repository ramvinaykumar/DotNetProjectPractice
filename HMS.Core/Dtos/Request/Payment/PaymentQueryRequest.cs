namespace HMS.Core.Dtos.Request.Payment
{
    public class PaymentQueryRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? BookingId { get; set; }
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
