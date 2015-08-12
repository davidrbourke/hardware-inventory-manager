using HardwareInventoryManager.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using HardwareInventoryManager.Models;
//using Microsoft.Owin.Security;
//using Owin;

namespace HardwareInventoryManager.Filters
{
    /// <summary>
    /// The ConfirmedFilter checks that the user has confirmed their email address before using the system
    /// </summary>
    public class ConfirmedFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ignore Anonymous actions
            if (IsActionAnonymous(filterContext))
            {
                return;
            }
            // Ignore allowable Authorized actions
            if (IsActionRequiredWithoutEmailConfirmation(filterContext))
            {
                return;
            }
            // Check if user email is confirmed
            if (!IsUserEmailConfirmed(filterContext))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Account", action = "EmailNotConfirmed" }));
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
        }

        private bool IsActionAnonymous(ActionExecutingContext filterContext)
        {
            return filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
        }

        private bool IsActionRequiredWithoutEmailConfirmation(ActionExecutingContext filterContext)
        {
            string action = filterContext.Controller.ControllerContext.RouteData.Values["action"].ToString();
            string controller = filterContext.Controller.ControllerContext.RouteData.Values["controller"].ToString();
            if (controller.Equals("Account", StringComparison.CurrentCultureIgnoreCase) &&
                (
                action.Equals("EmailNotConfirmed", StringComparison.CurrentCultureIgnoreCase) ||
                action.Equals("ConfirmEMail", StringComparison.CurrentCultureIgnoreCase) ||
                action.Equals("LogOff", StringComparison.CurrentCultureIgnoreCase)
                ))
            {
                return true;
            }
            return false;
        }

        private bool IsUserEmailConfirmed(ActionExecutingContext filterContext)
        {
            ApplicationUserManager appManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            Task<ApplicationUser> findUserTask = appManager.FindByEmailAsync(filterContext.HttpContext.User.Identity.Name);
            ApplicationUser user = findUserTask.Result as ApplicationUser;
            if (user != null)
            {
                Task<bool> confirmEmailTask = appManager.IsEmailConfirmedAsync(user.Id);
                bool isUserEmailConfirmed = confirmEmailTask.Result;
                return isUserEmailConfirmed;
            }
            return false;
        }
    }
}