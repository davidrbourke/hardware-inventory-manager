using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareInventoryManager.Models
{
    public class QuoteRequest: ModelEntity, ITenant
    {
        [Key]
        [Display(Name = "Quote Request Id")]
        public int QuoteRequestId { get; set; }

        [Display(Name = "Date Required")]
        public DateTime? DateRequired { get; set; }

        public int? Quantity { get; set; }

        [Display(Name = "Specification Details")]
        public string SpecificationDetails { get; set; }

        public int? QuoteResponseId { get; set; }
        [ForeignKey("QuoteResponseId")]
        public QuoteResponse QuoteResponse { get; set; }

        public int TenantId { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Lookup Category { get; set; }

        public int? QuoteRequestStatusId { get; set; }
        [ForeignKey("QuoteRequestStatusId")]
        public Lookup QuoteRequestStatus { get; set; }

        [NotMapped]
        public bool NewQuoteRequest { get; set; }
    }
}