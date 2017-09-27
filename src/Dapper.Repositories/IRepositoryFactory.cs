namespace Dapper.Repositories
{
    using System.Data;

    public interface IRepositoryFactory
    {
        IRepository<TEntity> Create<TEntity>(IDbConnection connection, IDbTransaction transaction = null) where TEntity : class, new();
    }
}