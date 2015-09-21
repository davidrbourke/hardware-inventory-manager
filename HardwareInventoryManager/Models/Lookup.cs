using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareInventoryManager.Models
{
    public class Lookup : ModelEntity
    {
        [Key]
        public int LookupId { get; set; }

        public string Description { get; set; }

        public int LookupTypeId { get; set; }
        [ForeignKey("LookupTypeId")]
        public LookupType Type { get; set; }

        public int AssociatedNumericValue { get; set; }

        public string AssociatedText { get; set; }

        public int? TenantId { get; set; }
        [ForeignKey("TenantId")]
        public Tenant Tenant { get; set; }
    }
}