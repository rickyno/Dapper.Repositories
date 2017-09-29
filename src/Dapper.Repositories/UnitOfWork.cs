namespace Dapper.Repositories
{
    using System;
    using System.Data;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;
        private bool disposed;

        public UnitOfWork(IDbConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.transaction = connection.BeginTransaction();
            this.repositoryFactory = new RepositoryFactory();
        }

        ~UnitOfWork()
        {
            this.Dispose(false);
        }

        public virtual IRepository<TEntity> Repository<TEntity>() where TEntity : class, new()
        {
            return this.repositoryFactory.Create<TEntity>(this.connection, this.transaction);
        }

        public virtual void Commit()
        {
            this.transaction?.Commit();
        }

        public virtual void Rollback()
        {
            this.transaction?.Rollback();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            transaction?.Dispose();
            connection?.Dispose();
            repositoryFactory?.Dispose();
            this.disposed = true;
        }
    }
}