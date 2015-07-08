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
        
        [Required]
        public string Model { get; set; }

        [Display(Name="Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name="Purchase Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }

        public int WarrantyPeriodId { get; set; }
        [ForeignKey("WarrantyPeriodId")]
        [Display(Name="Warranty Period")]
        public Lookup WarrantyPeriod { get; set; }

        [Display(Name="Obsolescense Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ObsolescenseDate { get; set; }

        [Display(Name="Price Paid")]
        public Decimal? PricePaid { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Lookup Category{ get; set; }

        [Display(Name="Location or Room")]
        public string LocationDescription { get; set; }
    }
}