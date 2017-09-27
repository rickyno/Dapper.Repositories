namespace Dapper.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, new();

        void Commit();

        void Rollback();
    }
}