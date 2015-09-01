using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HardwareInventoryManager.Models
{
    public class BulkImport : ModelEntity, ITenant
    {
        [Key]
        public int BulkImportId { get; set; }

        public string ImportText { get; set; }

        public int TenantId { get; set; }
    }
}