using BBS.Application.Interfaces.Infrastructure;
using BBS.Application.Interfaces.Repositories;
using BBS.Domain.Entities;
using Dapper;

namespace BBS.Infrastructure.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PassengerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateAsync(Passenger passenger)
        {
            const string sql = @"INSERT INTO bbs.Passenger
                                (
                                    FirstName,
                                    LastName,
                                    Email,
                                    PhoneNumber,
                                    Gender,
                                    DateOfBirth,
                                    IsActive
                                )
                                VALUES
                                (
                                    @FirstName,
                                    @LastName,
                                    @Email,
                                    @PhoneNumber,
                                    @Gender,
                                    @DateOfBirth,
                                    @IsActive
                                )

            SELECT CAST( SCOPE_IDENTITY() AS INT)";

            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QuerySingleAsync<int>(sql, passenger);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            const string sql = @"SELECT COUNT(*)
                                   FROM bbs.Passenger
                                  WHERE Email=@Email";

            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .ExecuteScalarAsync<int>(sql, new { Email = email }) > 0;
        }

        public async Task<bool> PhoneExistsAsync(string phone)
        {
            const string sql = @"SELECT COUNT(*)
                                   FROM bbs.Passenger
                                  WHERE PhoneNumber=@Phone";

            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .ExecuteScalarAsync<int>(sql, new { Phone = phone }) > 0;
        }

        public async Task<Passenger?> GetByIdAsync(int id)
        {
            const string sql =
                @"SELECT *
              FROM bbs.Passenger
              WHERE PassengerId=@Id";

            using var connection = _connectionFactory.CreateConnection();

            return await connection
                .QueryFirstOrDefaultAsync<Passenger>(
                    sql,
                    new { Id = id });
        }

        public async Task<IEnumerable<Passenger>> GetAllAsync()
        {
            const string sql = @"SELECT * FROM bbs.Passenger WHERE IsActive = 1";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Passenger>(sql);
        }

        public async Task<int> UpdateAsync(Passenger passenger)
        {
            const string sql = @"UPDATE bbs.Passenger
                                    SET FirstName = @FirstName,
                                        LastName = @LastName,
                                        Email = @Email,
                                        PhoneNumber = @Phone,
                                        DateOfBirth = @DateOfBirth,
                                  WHERE PassengerId = @PassengerId";

            using var connection = _connectionFactory.CreateConnection();

            return await connection.ExecuteAsync(sql, passenger);
        }

        public async Task<int> DeleteAsync(int passengerId)
        {
            const string sql = @"UPDATE bbs.Passenger
                                    SET IsActive = 0
                                  WHERE PassengerId=@PassengerId";
            using var connection = _connectionFactory.CreateConnection();
            return await connection
                .ExecuteAsync(sql, new { PassengerId = passengerId });
        }
    }
}
