using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Helpers.Account;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services.User
{
    public class UserServiceUoW
    {
        public EnumHelper.Roles UserRole { get; set; }
        public CustomApplicationDbContext DbContext { get; set; }
        public IRepository<ApplicationUser> UserRepository { get; set; }
        public IAccountProvider AccountProvider { get; set; }
        public int TenantId { get; set; }

        public UserServiceUoW(string userId, IAccountProvider accountProvider)
        {
            DbContext = new CustomApplicationDbContext();
            UserRole = GetUserRole(userId);
            UserRepository = new UserRepository(DbContext, userId, accountProvider);
            AccountProvider = accountProvider;
        }

        protected EnumHelper.Roles GetUserRole(string userId)
        {
            var store = new UserStore<ApplicationUser>(DbContext);
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
    }
}