using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Services.User;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace HardwareInventoryManager.Helpers.User
{
    public class AdminUserService : IUserService
    {
        private readonly CustomApplicationDbContext _context;

        public int TenantId{ get; set; }

        public AdminUserService(CustomApplicationDbContext context, int tenantId)
        {
            _context = context;
            TenantId = tenantId;
        }

        public IQueryable<Models.ApplicationUser> GetUsers()
        {
            return _context.Users.Include(t => t.UserTenants);
        }

        public Models.ApplicationUser GetUserById(string id)
        {
            ApplicationUser applicationUser = _context.Users.Include(u => u.UserTenants).FirstOrDefault(x => x.Id == id);
            return applicationUser;
        }

        public ApplicationUser EditUser(ApplicationUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return user;
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            ApplicationUser applicationUser =
        _context.Users.Include(u => u.UserTenants).FirstOrDefault(x => x.Email == email
            &&
            x.UserTenants.Any(t => t.TenantId == TenantId)
        );
            return applicationUser;
        }

        public EnumHelper.Roles GetCurrentUserRoleById(string userId)
        {
            var store = new UserStore<ApplicationUser>(_context);
            var manager = new UserManager<ApplicationUser>(store);
            EnumHelper.Roles userRole = EnumHelper.Roles.Viewer;
            if (manager.IsInRole(userId, EnumHelper.Roles.Admin.ToString()))
            {
                return EnumHelper.Roles.Admin;
            }
            else if (manager.IsInRole(userId, EnumHelper.Roles.Author.ToString()))
            {
                return EnumHelper.Roles.Author;
            }
            return userRole;
        }


        public ApplicationUser CreateUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}