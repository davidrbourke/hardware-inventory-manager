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

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Description { get; set; }
    }
}