using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace HardwareInventoryManager.ViewModels
{
    public class BulkUploadViewModel
    {
        public string BatchId { get; set; }
        public IEnumerable<AssetViewModel> Assets { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<List<string>> Errors { get; set; }
        public IEnumerable<TenantViewModel> Tenants { get; set; }
        public TenantViewModel SelectedTenant { get; set; }
    }
}