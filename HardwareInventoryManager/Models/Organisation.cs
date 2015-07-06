using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareInventoryManager.Models
{
    public class Organisation : ModelEntity
    {
        [Key]
        public int OrganisationId { get; set; }

        public string Name { get; set; }

        public Address OrganisationAddress { get; set; }

    }
}
