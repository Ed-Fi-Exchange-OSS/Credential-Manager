using System.Collections.Generic;
using System.Linq;
using Ed_Fi.Credential.Data;

namespace Ed_Fi.Credential.Business
{
    public interface IBusiness
    {
    }

    public interface IBaseBusiness<TEntity> : IBusiness
        where TEntity : class
    {
        IEnumerable<TEntity> Read();
        void Create(string wamsId, TEntity entity, bool save = true);
        void Update(string wamsId, TEntity entity, bool save = true);
        void Destroy(string wamsId, TEntity entity, bool save = true);
        TEntity Find<TId>(TId id);
        IQueryable<TEntity> Query();
        void SaveChanges(string wamsId);
    }

    public abstract class BaseBusiness<TEntity> : IBaseBusiness<TEntity> 
        where TEntity : class
    {
        public IDbContext Context { get; }

        protected BaseBusiness(IDbContext context)
         {
            Context = context;
        }

        public TEntity Find<TId>(TId id)
        {
            var entity = Context.FindEntity<TEntity, TId>(id);
            return entity;
        }

        public IQueryable<TEntity> Query()
        {
            var query = Context.Query<TEntity>();
            return query;
        }

        public IEnumerable<TEntity> Read()
        {
            var entity = Context.Query<TEntity>().ToList();
            return entity;
        }

        public void Create(string wamsId, TEntity entity, bool save = true)
        {

            Context.AddEntity(entity);

            if (save)
            {
                SaveChanges(wamsId);
            }
        }

        public void Update(string wamsId, TEntity entity, bool save = true)
        {
            Context.UpdateEntity(entity);

            if (save)
            {
                SaveChanges(wamsId);
            }
        }

        public void Destroy(string wamsId, TEntity entity, bool save = true)
        {
            Context.RemoveEntity(entity);

            if (save)
            {
                SaveChanges(wamsId);
            }
        }

        public void SaveChanges(string wamsId)
        {
            Context.SaveChanges(wamsId);
        }
    }
}
