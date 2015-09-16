using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HardwareInventoryManager.Models
{
    public class Contact : ModelEntity
    {
        [Key]
        public int ContactId { get; set; }

        [Required]
        [Display(Name="Your Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Your Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "How can we help?")]
        public string Description { get; set; }

        [Display(Name = "Message Read")]
        public bool? MessageRead { get; set; }
    }
}