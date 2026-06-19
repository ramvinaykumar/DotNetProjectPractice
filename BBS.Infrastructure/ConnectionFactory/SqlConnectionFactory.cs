using BBS.Application.Interfaces.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BBS.Infrastructure.ConnectionFactory
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _config;

        public SqlConnectionFactory(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            Console.WriteLine( $"Connection String = {connectionString}");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(  "DefaultConnection not found.");
            }

            return new SqlConnection(connectionString);
        }
    }
}
