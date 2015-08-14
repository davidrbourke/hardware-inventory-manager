using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace HardwareInventoryManager.Repository
{
    /// <summary>
    /// Generic asset repository. Ensures multi-tenancy constraints are enforced
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class, ITenant
    {
        private ApplicationUser _applicationUser;
        private CustomApplicationDbContext _db;

        public Repository()
            : this(new CustomApplicationDbContext())
        {
        }

        public Repository(CustomApplicationDbContext context)
        {
            _db = context;   
        }

        /// <summary>
        /// Get All the entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            IList<int> tenants = GetTenantIds();
            IQueryable<T> query = _db.Set<T>().Where(a =>  tenants.Contains(a.TenantId));
            //var tenants = _db..Assets.Include(a => a.Tenant).Include(a => a.AssetMake).Include(a => a.Category).Include(a => a.WarrantyPeriod).Where(t => userTenants.Contains(t.TenantId));
            return query;
        }

        /// <summary>
        /// Create a new entity if user has tenant permission
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public virtual T Create(T entity)
        {
            IList<int> tenants = GetTenantIds();
            if (tenants.Contains(entity.TenantId))
            {
                entity.TenantId = entity.TenantId;
                _db.Set<T>().Add(entity);
            }
            return entity;
        }

        /// <summary>
        /// Edit the entity if user has tenant permission
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public T Edit(T entity)
        {
            IList<int> tenants = GetTenantIds();
            if (tenants.Contains(entity.TenantId))
            {
                entity.TenantId = entity.TenantId;
                _db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            }
            return entity;
        }

        /// <summary>
        /// Returns entities with given filter if the user has tenant permission
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tenantId"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            IList<int> tenants = GetTenantIds();
            return _db.Set<T>().Where(predicate).Where(x => tenants.Contains(x.TenantId));
        }

        /// <summary>
        /// Delete the entity if the user has tenant permission
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tenantId"></param>
        public void Delete(T entity)
        {
            IList<int> tenants = GetTenantIds();
            if (GetTenantIds().Contains(entity.TenantId))
            {
                _db.Set<T>().Remove(entity);
            }
        }

        /// <summary>
        /// Returns entities with given filter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual T Single(Expression<Func<T, bool>> predicate)
        {
            IList<int> tenants = GetTenantIds();
            return _db.Set<T>().Where(predicate)
                .Where(x => tenants.Contains(x.TenantId)).FirstOrDefault();
        }

        /// <summary>
        /// Save the context to the DB
        /// </summary>
        public void Save()
        {
            _db.SaveChanges();
        }

        /// <summary>
        /// Load the Users tenant ids
        /// </summary>
        /// <returns>List of tenant ids</returns>
        private IList<int> GetTenantIds()
        {
            if(_applicationUser == null)
            {
                throw new Exception("Application User has not been set");
            }
            IList<int> tenantIdList = new List<int>();
            if (_applicationUser.UserTenants == null)
            {
                ApplicationUser reloadedUser = _db.Users.Include(u => u.UserTenants).FirstOrDefault(u => u.Id == _applicationUser.Id);
                tenantIdList = reloadedUser.UserTenants.Select(x => x.TenantId).ToList();
                if (tenantIdList.Count() == 0)
                {
                    throw new Exception("No tenant id has been found for the user");
                }
            } 
            else
            {
                tenantIdList = _applicationUser.UserTenants.Select(x => x.TenantId).ToList();
                if (tenantIdList.Count() == 0)
                {
                    throw new Exception("No tenant id has been found for the user");
                }
            }
            return tenantIdList;
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