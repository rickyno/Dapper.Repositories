namespace Dapper.Repositories
{
    using System;
    using System.Data;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;

        public UnitOfWork(IDbConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.transaction = connection.BeginTransaction();
            this.repositoryFactory = new RepositoryFactory();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, new()
        {
            return this.repositoryFactory.Create<TEntity>(this.connection, this.transaction);
        }

        public void Commit()
        {
            this.transaction.Commit();
        }

        public void Rollback()
        {
            this.transaction.Rollback();
        }
    }
}