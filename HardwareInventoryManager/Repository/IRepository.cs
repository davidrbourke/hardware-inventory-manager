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
        T Create(T t);
        T Edit(T t);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        void Delete(T t);
        void Save();
        void SetCurrentUser(ApplicationUser user);
        T Single(Expression<Func<T, bool>> predicate);   
    }
}
