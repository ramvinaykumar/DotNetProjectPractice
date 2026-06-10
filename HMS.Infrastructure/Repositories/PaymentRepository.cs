using Dapper;
using HMS.Core.Dtos.Request.Payment;
using HMS.Core.Interfaces;
using HMS.Core.Models.Dashboard;
using HMS.Core.Models.Payments;
using HMS.Infrastructure.Data;
using System.Data.Common;

namespace HMS.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDbConnectionFactory _db;
        public PaymentRepository(IDbConnectionFactory db) => _db = db;
        private DbConnection Conn() => (DbConnection)_db.CreateConnection();

        public async Task<(IEnumerable<Payment> Payments, int TotalCount)> GetAllAsync(PaymentQueryRequest q)
        {
            using var conn = Conn();
            var p = new DynamicParameters();
            p.Add("PageNumber", q.PageNumber); p.Add("PageSize", q.PageSize);
            p.Add("BookingId", q.BookingId); p.Add("CustomerId", q.CustomerId);
            p.Add("Status", q.Status);
            p.Add("FromDate", q.FromDate); p.Add("ToDate", q.ToDate);

            using var multi = await conn.QueryMultipleAsync("finance.usp_Payment_GetAll", p,
                commandType: System.Data.CommandType.StoredProcedure);
            var payments = await multi.ReadAsync<Payment>();
            var total = await multi.ReadFirstAsync<int>();
            return (payments, total);
        }

        public async Task<int> CreateAsync(PaymentCreateRequest r)
        {
            using var conn = Conn();
            return await conn.QueryFirstAsync<int>("finance.usp_Payment_Create",
                new
                {
                    r.BookingId,
                    r.CustomerId,
                    r.PaymentMethodId,
                    r.Amount,
                    r.Currency,
                    r.GatewayReference,
                    r.Notes
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> RefundAsync(int paymentId, string? notes)
        {
            using var conn = Conn();
            return await conn.QueryFirstAsync<int>("finance.usp_Payment_Refund",
                new { PaymentId = paymentId, Notes = notes },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PaymentMethod>> GetMethodsAsync()
        {
            using var conn = Conn();
            return await conn.QueryAsync<PaymentMethod>(
                "SELECT PaymentMethodId, MethodName, IsActive FROM finance.PaymentMethods WHERE IsActive = 1");
        }

        public async Task<IEnumerable<MonthlyRevenue>> GetMonthlyRevenueAsync(int? year)
        {
            using var conn = Conn();
            return await conn.QueryAsync<MonthlyRevenue>("finance.usp_Revenue_GetMonthly",
                new { Year = year },
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
