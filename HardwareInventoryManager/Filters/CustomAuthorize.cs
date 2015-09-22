using HardwareInventoryManager.Models;
using HardwareInventoryManager.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HardwareInventoryManager.Filters
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            
            EnumHelper.Roles role = EnumHelper.Roles.Viewer;
            if(HttpContext.Current.User.IsInRole(EnumHelper.Roles.Author.ToString()))
            {
                role = EnumHelper.Roles.Author;
            } else if(HttpContext.Current.User.IsInRole(EnumHelper.Roles.Admin.ToString()))
            {
                role = EnumHelper.Roles.Admin;
            }

            string action = filterContext.Controller.ControllerContext.RouteData.Values["action"].ToString();
            string controller = filterContext.Controller.ControllerContext.RouteData.Values["controller"].ToString();

            CustomApplicationDbContext context = new CustomApplicationDbContext();
            IQueryable<RolePermission> rolePermissions = context.RolePermissions;
            PermissionHelper permissionHelper = new PermissionHelper(rolePermissions);

            if (!permissionHelper.HasPermission(role, controller, action))
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
        }
    }
}