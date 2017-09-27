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
    }
}