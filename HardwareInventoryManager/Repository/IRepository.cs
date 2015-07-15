using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HardwareInventoryManager.Repository
{
    /// <summary>
    /// Generic Interface for CRUD functionality to interact with the database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T Create(T t, int tenantId);
        T Edit(T t, int tenantId);
        IQueryable<T> Find(int tenantId, Expression<Func<T, bool>> predicate);
        void Delete(T t, int tenantId);
        void Save();
        void SetCurrentUser(ApplicationUser user);
    }
}
