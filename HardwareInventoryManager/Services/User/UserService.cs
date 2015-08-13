using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HardwareInventoryManager.Services.User;

namespace HardwareInventoryManager.Helpers.User
{
    public class UserService : IUserService
    {
        private readonly CustomApplicationDbContext _context;

        public UserService(CustomApplicationDbContext context)
        {
            _context = context;
        }
        
        public IQueryable<ApplicationUser> GetUsers(int tenantId)
        {
            return _context.Users.Include(t => t.UserTenants)
                .Where(t => t.UserTenants.Where(x => x.TenantId == tenantId)
                .Any());
        }


        public ApplicationUser GetUser(int tenantId, string id)
        {
            ApplicationUser applicationUser = 
                _context.Users.Include(u => u.UserTenants).FirstOrDefault(x => x.Id == id
                    &&
                    x.UserTenants.Any(t => t.TenantId == tenantId)
                );
            return applicationUser;
        }


        public ApplicationUser EditUser(int tenantId, ApplicationUser user)
        {

            IList<Tenant> tenants = _context.Users.Include(t => t.UserTenants).FirstOrDefault(x => x.Id == user.Id).UserTenants.ToList();
            if (tenants.Any(x => x.TenantId == tenantId))
            {
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return user;
        }
    }
}