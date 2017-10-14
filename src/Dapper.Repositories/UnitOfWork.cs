namespace Dapper.Repositories
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Dapper.Repositories.Properties;

    public class UnitOfWork : IUnitOfWork
    {
        private IRepositoryFactory repositoryFactory;
        private IDbConnection connection;
        private IDbTransaction transaction;

        public UnitOfWork(IRepositoryFactory repositoryFactory, IDbConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection), Resources.ConnectionCannotBeNull);
            this.transaction = this.connection.BeginTransaction();
            this.repositoryFactory = repositoryFactory;
        }

        [ExcludeFromCodeCoverage]
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
            this.transaction.Commit();
        }

        public virtual void Rollback()
        {
            this.transaction.Rollback();
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            transaction?.Dispose();
            transaction = null;

            connection?.Dispose();
            connection = null;

            repositoryFactory?.Dispose();
            repositoryFactory = null;
        }
    }
}