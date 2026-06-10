using Dapper;
using HMS.Core.Dtos.Request.Customer;
using HMS.Core.Interfaces;
using HMS.Core.Models;
using HMS.Core.Models.Dashboard;
using HMS.Infrastructure.Data;

namespace HMS.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnectionFactory _db;

        public CustomerRepository(IDbConnectionFactory db) => _db = db;

        public async Task<(IEnumerable<Customer> Customers, int TotalCount)> GetAllAsync(CustomerQueryRequest q)
        {
            using var conn = _db.CreateConnection();
            var p = new DynamicParameters();
            p.Add("PageNumber", q.PageNumber);
            p.Add("PageSize", q.PageSize);
            p.Add("SearchTerm", q.SearchTerm);
            p.Add("IsVIP", q.IsVIP);
            p.Add("IsActive", q.IsActive);

            using var multi = await ((System.Data.Common.DbConnection)conn)
                .QueryMultipleAsync("hotel.usp_Customer_GetAll", p,
                    commandType: System.Data.CommandType.StoredProcedure);

            var customers = await multi.ReadAsync<Customer>();
            var total = await multi.ReadFirstAsync<int>();
            return (customers, total);
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            using var conn = _db.CreateConnection();
            using var multi = await ((System.Data.Common.DbConnection)conn)
                .QueryMultipleAsync("hotel.usp_Customer_GetById",
                    new { CustomerId = id },
                    commandType: System.Data.CommandType.StoredProcedure);
            return await multi.ReadFirstOrDefaultAsync<Customer>();
        }

        public async Task<(Customer? Customer, IEnumerable<BookingHistory> History)> GetByIdWithHistoryAsync(int id)
        {
            using var conn = _db.CreateConnection();
            using var multi = await ((System.Data.Common.DbConnection)conn)
                .QueryMultipleAsync("hotel.usp_Customer_GetById",
                    new { CustomerId = id },
                    commandType: System.Data.CommandType.StoredProcedure);
            var customer = await multi.ReadFirstOrDefaultAsync<Customer>();
            var history = await multi.ReadAsync<BookingHistory>();
            return (customer, history);
        }

        public async Task<int> CreateAsync(CustomerCreateRequest r)
        {
            using var conn = _db.CreateConnection();
            var result = await ((System.Data.Common.DbConnection)conn)
                .QueryFirstAsync<int>("hotel.usp_Customer_Create",
                    new
                    {
                        r.FirstName,
                        r.LastName,
                        r.Email,
                        r.PhoneNumber,
                        r.DateOfBirth,
                        r.Gender,
                        r.Nationality,
                        r.IDType,
                        r.IDNumber,
                        r.Address,
                        r.City,
                        r.Country,
                        r.Notes
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> UpdateAsync(int id, CustomerUpdateRequest r)
        {
            using var conn = _db.CreateConnection();
            var result = await ((System.Data.Common.DbConnection)conn)
                .QueryFirstAsync<int>("hotel.usp_Customer_Update",
                    new
                    {
                        CustomerId = id,
                        r.FirstName,
                        r.LastName,
                        r.Email,
                        r.PhoneNumber,
                        r.DateOfBirth,
                        r.Gender,
                        r.Nationality,
                        r.IDType,
                        r.IDNumber,
                        r.Address,
                        r.City,
                        r.Country,
                        r.IsVIP,
                        r.Notes
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = _db.CreateConnection();
            var result = await ((System.Data.Common.DbConnection)conn)
                .QueryFirstAsync<int>("hotel.usp_Customer_Delete",
                    new { CustomerId = id },
                    commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }
    }
}
