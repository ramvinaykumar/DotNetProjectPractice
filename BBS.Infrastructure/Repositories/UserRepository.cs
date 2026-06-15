using BBS.Application.Interfaces.Repositories;
using BBS.Domain.Entities;
using BBS.Infrastructure.ConnectionFactory;
using Dapper;

namespace BBS.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT * FROM bbs.Users WHERE Email=@Email";

            return await connection
                .QueryFirstOrDefaultAsync<User>(
                    sql,
                    new { Email = email });
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT * FROM  bbs.Users
                                  WHERE UserId=@UserId";

            return await connection
                .QueryFirstOrDefaultAsync<User>(
                    sql,
                    new { UserId = userId });
        }

        public async Task<int> CreateUserAsync(User user)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"INSERT INTO bbs.Users
                                (
                                    UserName,
                                    Email,
                                    PasswordHash,
                                    Role
                                )
                                VALUES
                                (
                                    @UserName,
                                    @Email,
                                    @PasswordHash,
                                    @Role
                                )

                                SELECT CAST(
                                    SCOPE_IDENTITY()
                                    AS INT)
                                ";

            return await connection
                .QuerySingleAsync<int>(
                    sql,
                    user);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT COUNT(*)
                                    FROM bbs.Users
                                   WHERE Email=@Email";

            var count = await connection
                    .ExecuteScalarAsync<int>(
                        sql,
                        new { Email = email });

            return count > 0;
        }

        public async Task SaveRefreshTokenAsync(RefreshToken token)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"

                            INSERT INTO bbs.RefreshToken
                            (
                                UserId,
                                TokenHash,
                                ExpiryDate,
                                IsRevoked
                            )
                            VALUES
                            (
                                @UserId,
                                @TokenHash,
                                @ExpiryDate,
                                @IsRevoked
                            )";

            await connection.ExecuteAsync(
                sql,
                token);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT *
                                   FROM bbs.RefreshToken
                                  WHERE TokenHash=@TokenHash";

            return await connection
                .QueryFirstOrDefaultAsync<
                    RefreshToken>(
                        sql,
                        new { TokenHash = token });
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"UPDATE bbs.RefreshToken
                                    SET IsRevoked=1
                                  WHERE TokenHash=@TokenHash";

            await connection.ExecuteAsync(
                sql,
                new { TokenHash = token });
        }
    }
}
