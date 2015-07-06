using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HardwareInventoryManager.Models
{
    public class LookupType : ModelEntity
    {
        [Key]
        public int LookupTypeId { get; set; }

        public string Description { get; set; }
    }
}