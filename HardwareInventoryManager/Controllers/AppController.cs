using HardwareInventoryManager.Models;
using HardwareInventoryManager.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using HardwareInventoryManager.Filters;


namespace HardwareInventoryManager.Controllers
{
    [ConfirmedFilter]
    public class AppController : Controller
    {
        private CustomApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public AppController()
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _context = new CustomApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        public void Alert(EnumHelper.Alerts alertType, string message)
        {
            TempData[alertType.ToString()] = message;
        }

        public ApplicationUser GetCurrentUser()
        {
            if (User != null)
            {
                return GetCurrentUser(User.Identity.Name);
            }
            return null;
        }

        public ApplicationUser GetCurrentUser(string email)
        {
            var user = _userManager.FindByName(email);
            ApplicationUser uu = _context.Users.Include(x => x.UserTenants).First(u => u.UserName == email) as ApplicationUser;
            return user;
        }

        public int GetTenantContextId()
        {
            int tenantContextId = 0;
            if(User != null)
            {
                ApplicationUser uu = _context.Users.Include(x => x.UserTenants).First(u => u.UserName == User.Identity.Name) as ApplicationUser;
                if (uu.UserTenants != null && uu.UserTenants.Count > 0)
                {
                    return uu.UserTenants.First().TenantId;
                }
            }
            return tenantContextId;
        }

        public int GetTenantIdFromEmail(string userEmailAddress)
        {
            int tenantContextId = 0;
            ApplicationUser uu = _context.Users.Include(x => x.UserTenants).First(u => u.UserName == userEmailAddress) as ApplicationUser;
            if (uu.UserTenants != null && uu.UserTenants.Count > 0)
            {
                return uu.UserTenants.First().TenantId;
            }
            return tenantContextId;
        }

        public EnumHelper.Roles GetUserRole()
        {

            var store = new UserStore<ApplicationUser>(_context);
            var manager = new UserManager<ApplicationUser>(store);
            EnumHelper.Roles userRole = EnumHelper.Roles.Viewer;
            if(manager.IsInRole(User.Identity.GetUserId(), EnumHelper.Roles.Admin.ToString()))
            {
                return EnumHelper.Roles.Admin;
            }
            else if (manager.IsInRole(User.Identity.GetUserId(), EnumHelper.Roles.Author.ToString()))
            {
                return EnumHelper.Roles.Author;
            }
            return userRole;
        }
    }
}