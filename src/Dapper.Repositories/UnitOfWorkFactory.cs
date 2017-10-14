namespace Dapper.Repositories
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private IRepositoryFactory repositoryFactory;

        public UnitOfWorkFactory(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public IUnitOfWork Create(IDbConnection connection)
        {
            return new UnitOfWork(this.repositoryFactory, connection);
        }

        [ExcludeFromCodeCoverage]
        ~UnitOfWorkFactory()
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
            if (!disposing)
            {
                return;
            }

            this.repositoryFactory?.Dispose();
            this.repositoryFactory = null;
        }
    }
}