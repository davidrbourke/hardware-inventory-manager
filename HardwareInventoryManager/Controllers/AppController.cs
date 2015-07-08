using HardwareInventoryManager.Models;
using HardwareInventoryManager.Services;
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

        public void Alert(EnumHelper.Alerts alertType, string message)
        {
            TempData[alertType.ToString()] = message;
        }
    }
}