using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareInventoryManager.Models
{
    public class QuoteRequest
    {
        [Key]
        public int QuoteRequestId { get; set; }

        public DateTime? DateRequired { get; set; }

        public int? Quantity { get; set; }

        public string SpecificationDetails { get; set; }

        public int? QuoteResponseId { get; set; }
        [ForeignKey("QuoteResponseId")]
        public QuoteResponse QuoteResponse { get; set; }
    }
}