using System.Data;

namespace BBS.Application.Interfaces.Infrastructure
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
