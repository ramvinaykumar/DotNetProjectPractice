using HMS.Core.Dtos.Request.Payment;
using HMS.Core.Models.Dashboard;
using HMS.Core.Models.Payments;

namespace HMS.Core.Interfaces
{
    public interface IPaymentRepository
    {
        Task<(IEnumerable<Payment> Payments, int TotalCount)> GetAllAsync(PaymentQueryRequest query);

        Task<int> CreateAsync(PaymentCreateRequest request);

        Task<int> RefundAsync(int paymentId, string? notes);

        Task<IEnumerable<PaymentMethod>> GetMethodsAsync();

        Task<IEnumerable<MonthlyRevenue>> GetMonthlyRevenueAsync(int? year);
    }
}
