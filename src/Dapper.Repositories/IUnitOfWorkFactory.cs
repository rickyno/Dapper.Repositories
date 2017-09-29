namespace Dapper.Repositories
{
    using System;
    using System.Data;

    public interface IUnitOfWorkFactory : IDisposable
    {
        IUnitOfWork Create(IDbConnection connection);
    }
}