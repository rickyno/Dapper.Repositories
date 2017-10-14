namespace Dapper.Repositories
{
    using System.Data;

    public interface IConnectionFactory
    {
        IDbConnection Create(string name = null);

        void Register(string name, string connectionString, bool defaultConnection = false);
    }
}