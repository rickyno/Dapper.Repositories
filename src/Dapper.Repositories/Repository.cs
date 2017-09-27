namespace Dapper.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Dapper.Contrib.Extensions;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly IDbConnection connection;
        private readonly IDbTransaction transaction;

        public Repository(IDbConnection connection, IDbTransaction transaction = null)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.transaction = transaction;
        }

        public TEntity Get(object id, int? timeout = null)
        {
            return this.connection.Get<TEntity>(id, this.transaction, timeout);
        }

        public IEnumerable<TEntity> Query(Func<TEntity, bool> filter = null, int? skip = null, int? take = null, int? timeout = null)
        {
            var query = this.connection.GetAll<TEntity>(this.transaction, timeout);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.Skip(skip ?? 0);

            if (take != null)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public void Add(TEntity entity, int? timeout = null)
        {
            this.connection.Insert(entity, this.transaction, timeout);
        }

        public void Add(IEnumerable<TEntity> entities, int? timeout = null)
        {
            this.connection.Insert(entities, this.transaction, timeout);
        }

        public void Update(TEntity entity, int? timeout = null)
        {
            this.connection.Update(entity, this.transaction, timeout);
        }

        public void Update(IEnumerable<TEntity> entities, int? timeout = null)
        {
            this.connection.Update(entities, this.transaction, timeout);
        }

        public void Delete(TEntity entity, int? timeout = null)
        {
            this.connection.Delete(entity, this.transaction, timeout);
        }

        public void Delete(IEnumerable<TEntity> entities, int? timeout = null)
        {
            this.connection.Delete(entities, this.transaction, timeout);
        }
    }
}
