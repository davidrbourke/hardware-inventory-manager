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
        public int QuoteRequestId { get; set; }

        public DateTime? DateRequired { get; set; }

        public string FormattedDateRequired 
        { 
            get 
            { 
                return DateRequired  == null ? string.Empty : DateRequired.Value.ToShortDateString();
            }
        }

        [Required]
        public string SpecificationDetails { get; set; }

        public IEnumerable<Lookup> ItemTypes { get; set; }

        [Required]
        public Lookup SelectedItemType { get; set; }

        public int? Quantity { get; set; }
        
        public IEnumerable<TenantViewModel> Tenants { get; set; }
        [Required]
        public TenantViewModel SelectedTenant { get; set; }
    }
}