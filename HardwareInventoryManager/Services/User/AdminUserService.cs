using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HardwareInventoryManager.Models;

namespace HardwareInventoryManager.Helpers.User
{
    public class AdminUserService : IUserService
    {
        private readonly CustomApplicationDbContext _context;

        public AdminUserService(CustomApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Models.ApplicationUser> GetUsers(int tenantId)
        {
            return _context.Users.Include(t => t.UserTenants);
        }


        public Models.ApplicationUser GetUser(int tenantId, string id)
        {
            ApplicationUser applicationUser = _context.Users.Include(u => u.UserTenants).FirstOrDefault(x => x.Id == id);
            return applicationUser;
        }


        public ApplicationUser EditUser(int tenantId, ApplicationUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return user;
        }
    }
}