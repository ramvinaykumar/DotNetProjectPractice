using System.ComponentModel.DataAnnotations;

namespace HMS.Core.Dtos.Request.Payment
{
    public class PaymentCreateRequest
    {
        [Required] public int BookingId { get; set; }
        [Required] public int CustomerId { get; set; }
        [Required] public int PaymentMethodId { get; set; }
        [Required, Range(0.01, double.MaxValue)] public decimal Amount { get; set; }
        [MaxLength(3)] public string Currency { get; set; } = "USD";
        public string? GatewayReference { get; set; }
        public string? Notes { get; set; }
    }
}
