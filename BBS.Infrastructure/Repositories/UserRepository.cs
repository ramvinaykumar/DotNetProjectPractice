using BBS.Application.Interfaces.Infrastructure;
using BBS.Application.Interfaces.Repositories;
using BBS.Domain.Entities;
using Dapper;

namespace BBS.Infrastructure.Repositories
{
    /// <summary>
    /// Provides data access methods for managing users and their refresh tokens in the database.
    /// </summary>
    /// <remarks>Implements the IUserRepository interface to support operations such as retrieving users by
    /// email or ID, creating users, checking for user existence, and handling refresh tokens.</remarks>
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Parameterized constructor for UserRepository, accepting a database connection factory to facilitate
        /// </summary>
        /// <param name="connectionFactory">IDbConnectionFactory connectionFactory</param>
        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Get email address of the user from the database asynchronously. Returns null if no user is found with the provided email.
        /// </summary>
        /// <param name="email">string email</param>
        /// <returns>A task representing the asynchronous operation, containing the user if found; otherwise, null.</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT * FROM bbs.Users WHERE Email=@Email";

            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        /// <summary>
        /// Get user details from the database by user ID asynchronously. Returns null if no user is found with the provided ID.
        /// </summary>
        /// <param name="userId">int userId</param>
        /// <returns>A task representing the asynchronous operation, containing the user if found; otherwise, null.</returns>
        public async Task<User?> GetByIdAsync(int userId)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT * FROM  bbs.Users
                                  WHERE UserId=@UserId";

            return await connection
                .QueryFirstOrDefaultAsync<User>(
                    sql, new { UserId = userId });
        }

        /// <summary>
        /// Asynchronously inserts a new user into the database and returns the generated user ID.
        /// </summary>
        /// <param name="user">The user entity containing the details to be added.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the newly created
        /// user.</returns>
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
                    sql, user);
        }

        /// <summary>
        /// Determines whether a user with the specified email address exists in the database.
        /// </summary>
        /// <param name="email">The email address to check for existence.</param>
        /// <returns>True if a user with the specified email exists; otherwise, false.</returns>
        public async Task<bool> UserExistsAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT COUNT(*)
                                    FROM bbs.Users
                                   WHERE Email=@Email";

            var count = await connection
                    .ExecuteScalarAsync<int>(
                        sql, new { Email = email });

            return count > 0;
        }

        /// <summary>
        /// Asynchronously persists a refresh token to the database.
        /// </summary>
        /// <param name="token">The refresh token to persist.</param>
        /// <returns>A task representing the asynchronous save operation.</returns>
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

            await connection.ExecuteAsync(sql, token);
        }

        /// <summary>
        /// Get refresh token details from the database by token hash asynchronously. 
        /// Returns null if no refresh token is found with the provided token hash.
        /// </summary>
        /// <param name="token">string token</param>
        /// <returns>Returns refresh token data</returns>
        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"SELECT *
                                   FROM bbs.RefreshToken
                                  WHERE TokenHash=@TokenHash";

            return await connection
                .QueryFirstOrDefaultAsync<
                    RefreshToken>(
                        sql, new { TokenHash = token });
        }

        /// <summary>
        /// Revokes a refresh token by marking it as revoked in the database.
        /// </summary>
        /// <param name="token">The hash of the refresh token to revoke.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task RevokeRefreshTokenAsync(string token)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"UPDATE bbs.RefreshToken
                                    SET IsRevoked=1
                                  WHERE TokenHash=@TokenHash";

            await connection.ExecuteAsync(
                sql, new { TokenHash = token });
        }
    }
}
