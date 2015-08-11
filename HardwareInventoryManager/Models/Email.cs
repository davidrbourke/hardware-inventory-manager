using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HardwareInventoryManager.Models
{
    public class Email : ModelEntity, ITenant
    {
        [Key]
        public int EmailId { get; set; }
        public string SenderEmailAddress { get; set; }
        public string RecipientsEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int TenantId { get; set; }
    }
}