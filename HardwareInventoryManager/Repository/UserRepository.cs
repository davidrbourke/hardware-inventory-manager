using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using HardwareInventoryManager.Services.Account;

namespace HardwareInventoryManager.Repository
{
    public class UserRepository : IRepository<ApplicationUser>
    {
        private string _userId;
        private CustomApplicationDbContext _db;
        private IAccountProvider _accountProvider;

        public UserRepository(string userName, IAccountProvider accountProvider)
            : this(new CustomApplicationDbContext(), userName, accountProvider)
        {
        }

        public UserRepository(CustomApplicationDbContext context, string userId, IAccountProvider accountProvider)
        {
            _db = context;
            _userId = userId;
            _accountProvider = accountProvider;
        }

        public IQueryable<ApplicationUser> GetAll()
        {
            IList<int> tenants = GetTenantIds();
            IQueryable<ApplicationUser> query = _db.Users.Where(
                u => u.UserTenants.Join(tenants,
                o => o.TenantId, i => i,
                (o, i) => new
                {
                    u
                }).FirstOrDefault().u.UserName == u.UserName).AsQueryable();
            return query;
        }

        private bool IsApplicationUserInCurrentUsersTenantList(ApplicationUser applicationUser)
        {
            IList<int> tenants = GetTenantIds();
            return applicationUser.UserTenants.Any(u => tenants.Contains(u.TenantId));
        }

        private bool IsPermissionAllowed(ApplicationUser applicationUser)
        {
            return (applicationUser.UserTenants != null && applicationUser.UserTenants.Count() > 0
                && IsApplicationUserInCurrentUsersTenantList(applicationUser));
        }

        public ApplicationUser Create(ApplicationUser applicationUser)
        {
            if (IsPermissionAllowed(applicationUser))
            {
                int tenantId = applicationUser.UserTenants.FirstOrDefault().TenantId;
                applicationUser.UserTenants = null;
                bool userSucceeded = _accountProvider.Create(applicationUser, applicationUser.TemporaryCode);
                bool roleSucceeded = _accountProvider.AddToRole(applicationUser.Id, applicationUser.TemporaryRole);
                Tenant tenant = _db.Tenants.FirstOrDefault(t => t.TenantId == tenantId);
                ApplicationUser reloadedUser = _db.Users.Include(t=>t.UserTenants).FirstOrDefault(u =>u.Id == applicationUser.Id);
                reloadedUser.UserTenants = new List<Tenant>
                    {
                        tenant
                    };
                _db.Entry(reloadedUser).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return applicationUser;
        }

        public ApplicationUser Edit(ApplicationUser applicationUser)
        {
            if (IsPermissionAllowed(applicationUser))
            {
                _db.Entry(applicationUser).State = EntityState.Modified;
            }
            return applicationUser;
        }

        public IQueryable<ApplicationUser> Find(System.Linq.Expressions.Expression<Func<ApplicationUser, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public void Delete(ApplicationUser applicationUser)
        {
            if (IsPermissionAllowed(applicationUser))
            {
                _db.Entry(applicationUser).State = EntityState.Deleted;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public ApplicationUser Single(System.Linq.Expressions.Expression<Func<ApplicationUser, bool>> predicate)
        {
            return GetAll().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Load the Users tenant ids
        /// </summary>
        /// <returns>List of tenant ids</returns>
        private IList<int> GetTenantIds()
        {

            IRepository<ApplicationUser> repository = new RepositoryWithoutTenant<ApplicationUser>();
            ApplicationUser applicationUser = repository.Single(u => u.Id == _userId);

            if (applicationUser == null)
            {
                throw new Exception("Application User has not been set");
            }
            IList<int> tenantIdList = new List<int>();
            if (applicationUser.UserTenants == null)
            {
                ApplicationUser reloadedUser = _db.Users.Include(u => u.UserTenants).FirstOrDefault(u => u.Id == applicationUser.Id);
                tenantIdList = reloadedUser.UserTenants.Select(x => x.TenantId).ToList();
                if (tenantIdList.Count() == 0)
                {
                    throw new Exception("No tenant id has been found for the user");
                }
            }
            else
            {
                tenantIdList = applicationUser.UserTenants.Select(x => x.TenantId).ToList();
                if (tenantIdList.Count() == 0)
                {
                    throw new Exception("No tenant id has been found for the user");
                }
            }
            return tenantIdList;
        }


        public void SetCurrentUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }


        public void SetCurrentUserByUsername(string userName)
        {
            throw new NotImplementedException();
        }
    }
}