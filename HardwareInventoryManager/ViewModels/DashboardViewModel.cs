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

        public int TotalAssets { get; set; }

        public int TotalWishlist { get; set; }

        public int TotalPendingUpgrade { get; set; }

        public IEnumerable<DashboardUpdates> DashboardUpdates { get; set;}

        public JArray AssetExpiryData { get; set; }

        public JArray AssetsByCategory { get; set; }
    }
}