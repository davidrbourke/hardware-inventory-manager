using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.ViewModels
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            DashboardUpdates = new List<DashboardUpdates>();
        }

        public int TotalAssets { get; set; }

        public int TotalWishlist { get; set; }

        public int TotalPendingUpgrade { get; set; }

        public IEnumerable<DashboardUpdates> DashboardUpdates { get; set;}
    }
}