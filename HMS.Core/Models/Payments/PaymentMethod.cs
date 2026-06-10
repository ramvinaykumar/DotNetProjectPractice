namespace HMS.Core.Models.Payments
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string MethodName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
