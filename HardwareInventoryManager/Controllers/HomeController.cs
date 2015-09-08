using HardwareInventoryManager.Filters;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Services;
using HardwareInventoryManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HardwareInventoryManager.Services.Assets;
using System.IO;
using Newtonsoft.Json.Linq;
using HardwareInventoryManager.Services.Dashboard;

namespace HardwareInventoryManager.Controllers
{
    [AllowAnonymous]
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

        [CustomAuthorize]
        [ConfirmedFilter]
        public ActionResult Dashboard()
        {
            DashboardViewModel dashboad = new DashboardViewModel();
            IQueryable<Asset> filteredAssets = AssetService.GetAllAssets();
            dashboad.TotalAssets = filteredAssets.Count();
            
            DashboardService dashboardService = new DashboardService(User.Identity.Name);
            IList<TwoColumnChartData> fourMonthExpiryData = dashboardService.AssetsByExpiry4Months();
            dashboad.AssetExpiryData = JArray.FromObject(fourMonthExpiryData);

            IList<TwoColumnChartData> pieChartData = dashboardService.AssetsByCategoryPieChart();
            dashboad.AssetsByCategory = JArray.FromObject(pieChartData);
            
            return View(dashboad);
        }


        private AssetService _assetService;
        public AssetService AssetService
        {
            get
            {
                if(_assetService == null)
                {
                    return new AssetService(User.Identity.Name);
                }
                return _assetService;
            }
            set
            {
                _assetService = value;
            }
        }
    }

    public class DataForChart
    {
        public string DateString { get; set; }
        public int CountOfAssets { get; set; }
    }

    public class AssetByCategoryForChart
    {
        public string Category { get; set; }
        public int CountOfAssets { get; set; }
    }

}