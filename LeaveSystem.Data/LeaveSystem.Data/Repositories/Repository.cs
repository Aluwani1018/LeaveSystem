using LeaveSystem.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LeaveSystem.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class 
    {
        protected internal ApplicationDbContext DbContext = null;
        protected internal DbSet<TEntity> DbSet;

        public Repository(IdentityDbContext<Employee, Role, string> dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            DbContext = dbContext as ApplicationDbContext;
            DbSet = DbContext.Set<TEntity>();
        }


         public IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> query =DbSet;
            var results = query.AsNoTracking().Where(where).AsQueryable();
            return results.AsNoTracking().ToList().ToList();
        }

         public IEnumerable<TEntity> GetAll()
        {
            return DbSet.AsNoTracking().AsQueryable().AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.AsNoTracking().Include(includeProperty).AsNoTracking();
            }

            return query.AsNoTracking().ToList();
        }
        public IEnumerable<TEntity> GetAllIncluding(int page,int pageSize,params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.AsNoTracking().Include(includeProperty).AsNoTracking();
            }
            if (page >= 0)
                query = query.Skip((page - 1) * pageSize);
            if (pageSize >= 0)
                query = query.Take(pageSize);

            return query.AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.AsNoTracking().Include(includeProperty).AsNoTracking();
            }

            return query.AsNoTracking().Where(where).AsNoTracking().ToList();
        }
        public IEnumerable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> where,int page,int pageSize, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.AsNoTracking().Include(includeProperty).AsNoTracking();
            }
            if (page >= 0)
                query = query.Skip((page - 1) * pageSize);
            if (pageSize >= 0)
                query = query.Take(pageSize);

            return query.AsNoTracking().Where(where).AsNoTracking().ToList();
        }

        public TEntity GetById(int id)
        {
            return DbSet.Find(id);
        }

        public void Add(TEntity entity)
        {
            var dbEntityEntry =DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var dbEntityEntry = DbContext.Entry(entity);

                if (dbEntityEntry.State != EntityState.Detached)
                {
                    dbEntityEntry.State = EntityState.Added;
                }
                else
                {
                    DbSet.Add(entity);
                }
            }

            //DbSet.AddRange(entities.Where(e => DbContext.Entry(e).State == EntityState.Added));
        }

        public virtual void Update(TEntity entity)
        {
            var dbEntityEntry = DbContext.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;
        }

    }
}
