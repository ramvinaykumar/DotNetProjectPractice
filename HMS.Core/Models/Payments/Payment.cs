namespace HMS.Core.Models.Payments
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string TransactionRef { get; set; } = string.Empty;
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string PaymentStatus { get; set; } = "Pending";
        public DateTime? PaidAt { get; set; }
        public string? Notes { get; set; }
        public string? GatewayReference { get; set; }
        public DateTime CreatedAt { get; set; }
        // From joins
        public string PaymentMethod { get; set; } = string.Empty;
        public string BookingReference { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
    }
}
