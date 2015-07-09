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
        
        [Required]
        public string Name { get; set; }

        public Address OrganisationAddress { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}