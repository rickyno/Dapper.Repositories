namespace Dapper.Repositories
{
    using System;
    using System.Data;

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            return new UnitOfWork(connection);
        }

        ~UnitOfWorkFactory()
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