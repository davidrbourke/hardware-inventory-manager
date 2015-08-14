using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace HardwareInventoryManager.Repository
{
    public class RepositoryWithoutTenant<T> : IRepository<T> where T : class
    {
        private ApplicationUser _applicationUser;
        private CustomApplicationDbContext _db;

        public RepositoryWithoutTenant()
            : this(new CustomApplicationDbContext())
        {
        }

        public RepositoryWithoutTenant(CustomApplicationDbContext context)
        {
            _db = context;   
        }

        /// <summary>
        /// Get All the entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            IQueryable<T> query = _db.Set<T>();
            return query;
        }

        /// <summary>
        /// Create a new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T Create(T entity)
        {
            _db.Set<T>().Add(entity);
            return entity;
        }

        /// <summary>
        /// Edit the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Edit(T entity)
        {
            _db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// Returns entities with given filter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().Where(predicate);
        }


        /// <summary>
        /// Returns entities with given filter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual T Single(Expression<Func<T, bool>> predicate)
        {
            return _db.Set<T>().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Delete the entity
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Save the context to the DB
        /// </summary>
        public void Save()
        {
            _db.SaveChanges();
        }

        /// <summary>
        /// Set the User
        /// </summary>
        /// <param name="user"></param>
        public void SetCurrentUser(ApplicationUser user)
        {
            _applicationUser = user;
        }
    }
}