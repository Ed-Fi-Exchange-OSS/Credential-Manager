using Ed_Fi.Credential.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace Ed_Fi.Credential.Domain.Model
{
    public partial class EdFiSecurityDbContext : IEdFiSecurityDbContext
    {
        public void AddEntity<T>(T entity) where T : class
        {
            Set<T>().Add(entity);
        }

        public void AttachEntity<T>(T entity) where T : class
        {
            if (!Exists(entity))
                Set<T>().Attach(entity);
        }

        public void UpdateEntity<T>(T entity) where T : class
        {
            AttachEntity(entity);
            SetEntityState(entity, EntityState.Modified);
        }

        public void SetEntityState<T>(T entity, EntityState state) where T : class
        {
            Entry(entity).State = state;
        }

        public T FindEntity<T, TId>(TId id) where T : class
        {
            var entity = Set<T>().Find(id);
            return entity;
        }

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
        {
            Set<T>().RemoveRange(entities);
        }

        public void RemoveEntity<T>(T entity) where T : class
        {
            AttachEntity(entity);
            Set<T>().Remove(entity);
        }

        public void RemoveEntity<T, TId>(TId id) where T : class
        {
            var entity = FindEntity<T, TId>(id);
            Set<T>().Remove(entity);
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>();
        }

        public bool Exists<T>(T entity) where T : class
        {
            return Set<T>().Local.Any(e => e == entity);
        }

        void IDbContext.SaveChanges(string wamsId)
        {
            try
            {
                SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Debug.Write(ex.ToString());
            }
            catch (DbUpdateException ex)
            {
                Debug.Write(ex.ToString());
            }
        }
    }

    public interface IEdFiSecurityDbContext : IEdFiSecurityDbContextBase, IDbContext
    {

    }
}