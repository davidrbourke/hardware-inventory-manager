using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

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

        public IQueryable<QuoteRequest> GetAllQuote()
        {
            return _quoteRequestRepository.GetAll();
        }

        public QuoteRequest GetSingleQuote(int id)
        {
            return _quoteRequestRepository.GetAll().Include(q => q.Category).FirstOrDefault(q => q.QuoteRequestId == id);
        }
    }
}