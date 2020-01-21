using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Ed_Fi.Credential.Data
{
    public interface IDbContext : IDisposable
    {
        void AddEntity<T>(T entity) where T : class;
        void AttachEntity<T>(T entity) where T : class;
        void UpdateEntity<T>(T entity) where T : class;
        void SetEntityState<T>(T entity, EntityState state) where T : class;
        T FindEntity<T, TId>(TId id) where T : class;
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;
        void RemoveEntity<T>(T entity) where T : class;
        void RemoveEntity<T, TId>(TId id) where T : class;
        IQueryable<T> Query<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
        bool Exists<T>(T entity) where T : class;
        void SaveChanges(string wamsId);
    }
}
