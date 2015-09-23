using HardwareInventoryManager.Filters;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HardwareInventoryManager.Helpers.Assets;
using System.IO;
using Newtonsoft.Json.Linq;
using HardwareInventoryManager.Helpers.Dashboard;
using System.Web.Security;
using Microsoft.AspNet.Identity;

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

            string userId = User.Identity.GetUserId();

            DashboardService dashboardService = new DashboardService(User.Identity.Name);
            IList<TwoColumnChartData> fourMonthExpiryData = dashboardService.AssetsByExpiry4Months();
            dashboad.AssetExpiryData = JArray.FromObject(fourMonthExpiryData);

            IList<TwoColumnChartData> pieChartData = dashboardService.AssetsByCategoryPieChart();
            dashboad.AssetsByCategory = JArray.FromObject(pieChartData);

            IList<TwoColumnChartData> warrantyExpiryData = dashboardService.AssetsWarrantyExpiry4Months();
            dashboad.WarrantyExpiryData = JArray.FromObject(warrantyExpiryData);

            int[] wishListStatus = dashboardService.WishListSummary();
            dashboad.TotalWishlistPending = wishListStatus[0];
            dashboad.TotalWishlistProcessing = wishListStatus[1];
            dashboad.TotalWishlistSupplied = wishListStatus[2];
            dashboad.TotalWishlistComplete = wishListStatus[3];
            dashboad.TotalWishlist = wishListStatus[0] + wishListStatus[1] + wishListStatus[2] + wishListStatus[3];

            var dashboardCollection = dashboardService.DisplayPanels(userId);
            dashboad.DisplayButtonsPanel = bool.Parse(dashboardCollection[EnumHelper.ApplicationSettingKeys.DashboardButtonsPanel.ToString()]);
            dashboad.DisplayNotificationsPanel = bool.Parse(dashboardCollection[EnumHelper.ApplicationSettingKeys.DashboardNotificationsPanel.ToString()]);
            dashboad.DisplayAssetPieChartPanel = bool.Parse(dashboardCollection[EnumHelper.ApplicationSettingKeys.DashboardAssetsPieChartPanel.ToString()]);
            dashboad.DisplayAssetObsoletePanel = bool.Parse(dashboardCollection[EnumHelper.ApplicationSettingKeys.DashboardAssetsObsoleteChartPanel.ToString()]);
            dashboad.DisplayAssetWarrantyPanel= bool.Parse(dashboardCollection[EnumHelper.ApplicationSettingKeys.DashboardAssetsWarrantyExpiryChartPanel.ToString()]);
            dashboad.DisplayWatchlistStatsPanel = bool.Parse(dashboardCollection[EnumHelper.ApplicationSettingKeys.DashboardAssetsWishlistStatsPanel.ToString()]);


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