using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HardwareInventoryManager.Models
{
    public class DashboardUpdates : ModelEntity
    {
        [Key]
        public int DashboadUpdatesId { get; set; }

        public DateTime MessageDate { get; set; }

        public string Message { get; set; }

        public bool Complete { get; set; }
    }
}