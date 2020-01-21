using Ed_Fi.Credential.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Ed_Fi.Credential.Domain.Model
{
    public interface IEdFiAdminDbContext : IEdFiAdminDbContextBase, IDbContext
    {

    }

    public partial class EdFiAdminDbContext : IEdFiAdminDbContext
    {
        partial void InitializePartial()
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 60;
        }

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

        //ObjectContext IObjectContextAdapter.ObjectContext => this is IObjectContextAdapter objectContext ? objectContext.ObjectContext : null;

        void IDbContext.SaveChanges(string wamsId)
        {
            var changes = ChangeTracker
                .Entries()
                .Where(e => e.State != EntityState.Unchanged);

            var auditEntries = new List<AuditEntry>();

            foreach (var change in changes)
            {
                var type = ObjectContext.GetObjectType(change.Entity.GetType());
                //var proxytype = change.Entity.GetType();

                var auditEntry = new AuditEntry
                {
                    CreatedBy = wamsId ?? "SYSTEM",
                    StateName = change.State.ToString(),
                    State = (int)change.State,
                    EntityTypeName = type.Name,
                    CreatedDate = DateTime.Now
                };
                var properties = change.State == EntityState.Deleted ? 
                    change.OriginalValues : 
                    change.CurrentValues;

                foreach (var propertyName in properties.PropertyNames)
                {  
                    var auditEntryProperty = new AuditEntryProperty
                    {
                        PropertyName = propertyName
                    };

                    if (change.State == EntityState.Modified || change.State == EntityState.Added)
                    {
                        var currentValues = change.CurrentValues;
                        var current = currentValues[propertyName];
                        auditEntryProperty.NewValue = current?.ToString() ?? "";
                    }

                    if (change.State == EntityState.Modified || change.State == EntityState.Deleted)
                    {
                        var originalValues = change.OriginalValues;
                        var original = originalValues[propertyName];
                        auditEntryProperty.OldValue = original?.ToString() ?? "";
                    }

                    auditEntry.AuditEntryProperties.Add(auditEntryProperty);
                }

                auditEntries.Add(auditEntry);
            }

            auditEntries.ForEach(a => AuditEntries.Add(a));

            SaveChanges();
        }
    }
}