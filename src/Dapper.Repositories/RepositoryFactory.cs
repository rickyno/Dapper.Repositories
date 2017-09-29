namespace Dapper.Repositories
{
    using System;
    using System.Data;

    public class RepositoryFactory : IRepositoryFactory
    {
        public IRepository<TEntity> Create<TEntity>(IDbConnection connection, IDbTransaction transaction = null) where TEntity : class, new()
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            return new Repository<TEntity>(connection, transaction);
        }

        ~RepositoryFactory()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}