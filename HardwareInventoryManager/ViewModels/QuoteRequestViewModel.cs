using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.ViewModels
{
    public class QuoteRequestViewModel
    {
        [Display(Name="Quote Request Item")]
        public int QuoteRequestId { get; set; }

        public DateTime? DateRequired { get; set; }

        public string FormattedDateRequired 
        { 
            get 
            { 
                return DateRequired  == null ? string.Empty : DateRequired.Value.ToShortDateString();
            }
        }

        [Display(Name ="Specification Details")]
        [Required]
        public string SpecificationDetails { get; set; }

        public IEnumerable<Lookup> ItemTypes { get; set; }

        [Required]
        public Lookup SelectedItemType { get; set; }
            
        public int? Quantity { get; set; }
        
        public IEnumerable<TenantViewModel> Tenants { get; set; }
        [Required]
        public TenantViewModel SelectedTenant { get; set; }

        public Lookup Category { get; set; }

        public string SpecificationSummary
        {
            get
            {
                string specificationSummary = string.Empty;
                if (string.IsNullOrWhiteSpace(SpecificationDetails))
                    return string.Empty;

                return SpecificationDetails.Length <= 33 ?
                    SpecificationDetails :
                    string.Format("{0}...", SpecificationDetails.Substring(0, 30));
            }
        }

        public IEnumerable<Lookup> QuoteRequestStatuses { get; set; }

        [Required]
        public Lookup SelectedQuoteRequestStatus { get; set; }

        public bool CanChangeStatus { get; set; }

        public bool NewQuoteRequest { get; set; }

        public string PanelClass { 
            get
            {
                if(NewQuoteRequest) return "panel-info";
                return "panel-default";
            }

            }
    }
}