using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace HardwareInventoryManager.Services
{
    public static class CustomViewHelpers
    {
        /// <summary>
        /// Returns the table row Edit | Details | Delete links which the user in scope has permission to.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="linkId"></param>
        /// <returns></returns>
        public static MvcHtmlString IndexLinks(this HtmlHelper html, object linkId)
        {
            // Get the users role
            EnumHelper.Roles role = EnumHelper.Roles.Viewer;
            if (HttpContext.Current.User.IsInRole(EnumHelper.Roles.Author.ToString()))
            {
                role = EnumHelper.Roles.Author;
            }
            else if (HttpContext.Current.User.IsInRole(EnumHelper.Roles.Admin.ToString()))
            {
                role = EnumHelper.Roles.Admin;
            }

            // Get the current route Controller name
            var currentRouteData = html.ViewContext.RouteData;
            string controller = currentRouteData.GetRequiredString("controller");
            
            // Check user permissions
            CustomApplicationDbContext context = new CustomApplicationDbContext();
            IQueryable<RolePermission> rolePermissions = context.RolePermissions;
            PermissionHelper permissionHelper = new PermissionHelper(rolePermissions);

            // Build the MvcHtmlString
            StringBuilder linkStringBuilder = new StringBuilder();
            if (permissionHelper.HasPermission(role, controller, "Edit"))
            {
                linkStringBuilder.Append(string.Format("{0} |",
                    LinkExtensions.ActionLink(html, "Edit", "Edit", linkId)));
            }
            linkStringBuilder.Append(LinkExtensions.ActionLink(html, "Details", "Details", linkId));
            if (permissionHelper.HasPermission(role, controller, "Delete"))
            {
                linkStringBuilder.Append(string.Format("| {0}",
                    LinkExtensions.ActionLink(html, "Delete", "Delete", linkId)));
            }
            return new MvcHtmlString(linkStringBuilder.ToString());
        }
    }
}