using HardwareInventoryManager.Models;
using HardwareInventoryManager.Services;
using HardwareInventoryManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardwareInventoryManager.Controllers
{
    public class HomeController : AppController
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Dashboard()
        {
            int tenant = TenantHelper.LoadTenant(User.Identity.Name);
            CustomApplicationDbContext context = new CustomApplicationDbContext();
            DashboardViewModel dashboad = new DashboardViewModel();
            dashboad.DashboardUpdates = context.DashboardUpdates;
            dashboad.TotalAssets = context.Assets.Count(a => a.TenantId == tenant);
            return View(dashboad);
        }
    }
}