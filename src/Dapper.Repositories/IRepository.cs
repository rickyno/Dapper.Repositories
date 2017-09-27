namespace Dapper.Repositories
{
    using System;
    using System.Collections.Generic;

    public interface IRepository<TEntity> where TEntity : class, new()
    {
        TEntity Get(object id, int? timeout = null);

        IEnumerable<TEntity> Query(Func<TEntity, bool> filter = null, int? skip = null, int? take = null, int? timeout = null);

        void Add(TEntity entity, int? timeout = null);

        void Add(IEnumerable<TEntity> entities, int? timeout = null);

        void Update(TEntity entity, int? timeout = null);

        void Update(IEnumerable<TEntity> entities, int? timeout = null);

        void Delete(TEntity entity, int? timeout = null);

        void Delete(IEnumerable<TEntity> entities, int? timeout = null);
    }
}