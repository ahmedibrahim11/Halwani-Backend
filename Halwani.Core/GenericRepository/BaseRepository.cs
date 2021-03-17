using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Halwani.Data;

namespace Halawani.Core
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private HalawaniContext _context = null;
        protected DbSet<TEntity> _dbSet;

        public BaseRepository()
        {
            _context = new HalawaniContext();
            _dbSet = _context.Set<TEntity>();
        }
        public TEntity GetById(object id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
                _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }
        public IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public IEnumerable<TEntity> GetByAll()
        {
            return _dbSet.ToList<TEntity>();
        }
        public TEntity Add(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;  //amr change 
            //EntityEntry < TEntity >
            //return _dbSet.Add(entity);
        }
        public void AddRange(List<TEntity> entities)
        {
            _dbSet.AddRange(entities);  //amr change 
            //EntityEntry < TEntity >
            //return _dbSet.Add(entity);
        }
        public TEntity Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return entityToUpdate;
        }
        public void RemoveById(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Remove(entityToDelete);
        }
        public void Remove(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }
        public void RemoveRange(IEnumerable<TEntity> entity)
        {
            _dbSet.RemoveRange(entity);
        }
        public int Count()
        {
          return  _dbSet.Count();
        }
        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
