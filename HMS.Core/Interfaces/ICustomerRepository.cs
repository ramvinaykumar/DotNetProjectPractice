using HMS.Core.Dtos.Request.Customer;
using HMS.Core.Models;
using HMS.Core.Models.Dashboard;

namespace HMS.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<(IEnumerable<Customer> Customers, int TotalCount)> GetAllAsync(CustomerQueryRequest query);

        Task<Customer?> GetByIdAsync(int customerId);

        Task<(Customer? Customer, IEnumerable<BookingHistory> History)> GetByIdWithHistoryAsync(int customerId);

        Task<int> CreateAsync(CustomerCreateRequest request);

        Task<int> UpdateAsync(int customerId, CustomerUpdateRequest request);

        Task<int> DeleteAsync(int customerId);
    }
}
