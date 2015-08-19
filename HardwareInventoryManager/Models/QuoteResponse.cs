using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareInventoryManager.Models
{
    public class QuoteResponse : ModelEntity, ITenant
    {
        [Key, ForeignKey("QuoteRequest")]
        public int QuoteReposonseId { get; set; }

        public decimal? QuoteCostPerItem { get; set; }

        public decimal? QuoteCostTotal { get; set; }

        public string Notes { get; set; }

        public int QuoteRequestId { get; set; }
        [ForeignKey("QuoteRequestId")]
        public QuoteRequest QuoteRequest { get; set; }

        public int TenantId { get; set; }
    }
}