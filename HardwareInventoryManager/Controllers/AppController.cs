using HardwareInventoryManager.Models;
using HardwareInventoryManager.Services;
using Microsoft.AspNet.Identity.EntityFramework;
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

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        public void Alert(EnumHelper.Alerts alertType, string message)
        {
            TempData[alertType.ToString()] = message;
        }

        public IList<int> LoadTenants()
        {
            CustomApplicationDbContext context = new CustomApplicationDbContext();
            
            ApplicationUser user = context.Users.Include(x =>x.UserTenants).First(u => u.UserName == User.Identity.Name) as ApplicationUser;
            
            return user.UserTenants.Select(x => x.TenantId).ToList();
        }
    }
}