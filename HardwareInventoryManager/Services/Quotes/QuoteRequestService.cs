using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services.Quotes
{
    public class QuoteRequestService
    {
        private IRepository<QuoteRequest> _quoteRequestRepository;

        public QuoteRequestService(IRepository<QuoteRequest> quoteRequestRepository)
        {
            _quoteRequestRepository = quoteRequestRepository;
            
        }

        public void CreateQuoteRequest(QuoteRequest quoteRequest)
        {
            _quoteRequestRepository.Create(quoteRequest);
            _quoteRequestRepository.Save();
        }

        
    }
}