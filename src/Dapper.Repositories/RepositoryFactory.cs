namespace Dapper.Repositories
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;

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

        [ExcludeFromCodeCoverage]
        ~RepositoryFactory()
        {
            this.Dispose(false);
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
        }
    }
}