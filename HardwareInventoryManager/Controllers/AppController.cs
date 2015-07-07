using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
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

        public Tenant LoadTenant()
        {
            CustomApplicationDbContext context = new CustomApplicationDbContext();
            ApplicationUser user  = context.Users.First(u => u.UserName == this.HttpContext.User.Identity.Name);
            return user.Tenant;
        }
    }
}