using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.ViewModels
{
    public class BulkUploadViewModel
    {
        public string BatchId { get; set; }
        public IEnumerable<AssetViewModel> Assets { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}