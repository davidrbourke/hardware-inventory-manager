using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HardwareInventoryManager.Models
{
    public class RolePermission : ModelEntity
    {
        [Key]
        public int RolePersmissionId { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Role { get; set; }
    }
}