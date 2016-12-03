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
        [Display(Name = "Quote Response Id")]
        [Key, ForeignKey("QuoteRequest")]
        public int QuoteReposonseId { get; set; }

        [Display(Name = "Quote Cost Per Item")]
        public decimal? QuoteCostPerItem { get; set; }

        [Display(Name = "Quote Cost Total")]
        public decimal? QuoteCostTotal { get; set; }

        public string Notes { get; set; }

        public int QuoteRequestId { get; set; }
        [Display(Name = "Quote Request")]
        [ForeignKey("QuoteRequestId")]
        public QuoteRequest QuoteRequest { get; set; }

        public int TenantId { get; set; }
    }
}