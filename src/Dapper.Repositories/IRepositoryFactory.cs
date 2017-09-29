namespace Dapper.Repositories
{
    using System;
    using System.Data;

    public interface IRepositoryFactory : IDisposable
    {
        IRepository<TEntity> Create<TEntity>(IDbConnection connection, IDbTransaction transaction = null) where TEntity : class, new();
    }
}