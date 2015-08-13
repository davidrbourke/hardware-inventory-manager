using HardwareInventoryManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HardwareInventoryManager.Helpers;

namespace HardwareInventoryManager.Services.User
{
    /// <summary>
    /// Utility class for common user methods
    /// </summary>
    public class UserUtility : IUserUtility
    {
        private CustomApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public UserUtility()
        {
            _context = new CustomApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        /// <summary>
        /// Returns an ApplicationUser object for the given userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApplicationUser GetUserById(string userId)
        {
            var user = _userManager.FindById(userId);
            ApplicationUser uu = _context.Users.Include(x => x.UserTenants).First(u => u.Id == userId) as ApplicationUser;
            return user;
        }

        /// <summary>
        /// Returns an ApplicationUser object for the given userEmail address
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public ApplicationUser GetUserByEmail(string userEmail)
        {
            var user = _userManager.FindByName(userEmail);
            ApplicationUser uu = _context.Users.Include(x => x.UserTenants).First(u => u.UserName == userEmail) as ApplicationUser;
            return user;
        }

        /// <summary>
        /// Returns an Enum of Role for the given userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Helpers.EnumHelper.Roles GetCurrentUserRoleById(string userId)
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
    }
}