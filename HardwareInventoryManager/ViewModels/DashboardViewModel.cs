using HardwareInventoryManager.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.ViewModels
{
    public class DashboardViewModel : AppViewModel
    {
        public DashboardViewModel()
        {
            DashboardUpdates = new List<DashboardUpdates>();
        }

        public int TotalWishlist { get; set; }

        public int TotalWishlistPending { get; set; }
        public int TotalWishlistProcessing { get; set; }
        public int TotalWishlistSupplied { get; set; }
        public int TotalWishlistComplete { get; set; }

        public int TotalPendingUpgrade { get; set; }

        public IEnumerable<DashboardUpdates> DashboardUpdates { get; set;}

        public JArray AssetExpiryData { get; set; }

        public JArray AssetsByCategory { get; set; }

        public JArray WarrantyExpiryData { get; set; }

        public bool DisplayButtonsPanel { get; set; }
        public bool DisplayNotificationsPanel { get; set; }
        public bool DisplayAssetPieChartPanel { get; set; }
        public bool DisplayAssetObsoletePanel { get; set; }
        public bool DisplayAssetWarrantyPanel { get; set; }
        public bool DisplayWatchlistStatsPanel { get; set; }

    }
}
