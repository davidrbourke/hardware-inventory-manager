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


namespace HardwareInventoryManager.Controllers
{
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
                var user = _userManager.FindByName(User.Identity.Name);
                ApplicationUser uu = _context.Users.Include(x => x.UserTenants).First(u => u.UserName == User.Identity.Name) as ApplicationUser;
                return user;
            }
            return null;
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
    }
}