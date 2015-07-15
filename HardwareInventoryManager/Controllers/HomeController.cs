using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
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
            IRepository<Asset> rep;

            rep = new Repository<Asset>();
            rep.SetCurrentUser(GetCurrentUser());
            CustomApplicationDbContext context = new CustomApplicationDbContext();
            DashboardViewModel dashboad = new DashboardViewModel();
            dashboad.DashboardUpdates = context.DashboardUpdates;

            var filteredAssets = rep.GetAll();
            dashboad.TotalAssets = filteredAssets.Count();
            return View(dashboad);
        }
    }
}