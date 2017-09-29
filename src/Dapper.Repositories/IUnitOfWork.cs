namespace Dapper.Repositories
{
    using System;
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, new();

        void Commit();

        void Rollback();
    }
}