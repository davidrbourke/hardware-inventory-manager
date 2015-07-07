using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace HardwareInventoryManager.Models
{
    public class Asset : ModelEntity
    {
        [Key]
        public int AssetId { get; set; }

        public int TenantId { get; set; }
        [ForeignKey("TenantId")]
        public Tenant Tenant { get; set; } 

        public int AssetMakeId { get; set; }
        [ForeignKey("AssetMakeId")]
        public Lookup AssetMake { get; set; }
        
        public string Model { get; set; }

        public string SerialNumber { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public int WarrantyPeriodId { get; set; }
        [ForeignKey("WarrantyPeriodId")]
        public Lookup WarrantyPeriod { get; set; }

        public DateTime? ObsolescenseDate { get; set; }

        public Decimal PricePaid { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Lookup Category{ get; set; }

        public string LocationDescription { get; set; }
    }
}