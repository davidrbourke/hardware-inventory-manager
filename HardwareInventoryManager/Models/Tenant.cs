using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Models
{
    public class Tenant : ModelEntity
    {
        [Key]
        public int TenantId { get; set; }

        public int TenantOrganisationId { get; set; }
        [ForeignKey("TenantOrganisationId")]
        public Organisation TenantOrganisation { get; set; }
    }
}