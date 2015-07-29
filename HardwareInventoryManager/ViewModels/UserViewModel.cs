using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name="Email Address")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Lockout Enabled")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Access Failed Count")]
        public int AccessFailedCount { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public ICollection<Tenant> UserTenants { get; set; }
    }
}