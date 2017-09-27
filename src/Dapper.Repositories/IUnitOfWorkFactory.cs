namespace Dapper.Repositories
{
    using System.Data;

    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(IDbConnection connection);
    }
}