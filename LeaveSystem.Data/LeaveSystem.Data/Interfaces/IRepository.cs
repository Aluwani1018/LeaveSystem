using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LeaveSystem.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetAllIncluding(int page, int pageSize,params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetAllIncluding(Expression<Func<TEntity, bool>> where, int page, int pageSize, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetById(int id);
        void Add(TEntity entity);
        void Add(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> where);

    }
}
