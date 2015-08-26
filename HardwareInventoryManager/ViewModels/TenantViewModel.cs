using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.ViewModels
{
    public class TenantViewModel
    {
        public int TenantId { get; set; }

        [Required]
        public string Name { get; set; }

        public Address OrganisationAddress { get; set; }
    }
}