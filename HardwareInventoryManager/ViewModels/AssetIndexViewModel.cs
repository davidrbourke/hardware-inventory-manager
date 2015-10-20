using HardwareInventoryManager.Controllers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.ViewModels
{
    public class AssetIndexViewModel
    {
        public JObject AssetListJson { get; set; }

        public string ReportDisplayName { get; set; }

        public string ReportDescription { get; set; }

        public JObject ReportHeaders { get; set; }

        public IEnumerable<Header> Headers { get; set; }
    }
}