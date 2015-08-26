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
        public string UserName { get; set; }

        public QuoteRequestService(string userName)
        {
            UserName = userName;
        }
        
        public void SaveQuoteRequest(QuoteRequest quoteRequest)
        {
            if(quoteRequest.QuoteRequestId != 0)
            {
                UpdateQuoteRequest(quoteRequest);
                return;
            }
            CreateQuoteRequest(quoteRequest);
        }

        public void CreateQuoteRequest(QuoteRequest quoteRequest)
        {
            Repository.Create(quoteRequest);
            Repository.Save();
        }

        public void UpdateQuoteRequest(QuoteRequest quoteRequest)
        {
            Repository.Edit(quoteRequest);
            Repository.Save();
        }

        public void DeleteQuoteRequest(int quoteRequestId)
        {
            QuoteRequest quoteRequestToDelete = GetSingleQuote(quoteRequestId);
            Repository.Delete(quoteRequestToDelete);
            Repository.Save();
        }

        public IQueryable<QuoteRequest> GetAllQuotes()
        {
            return Repository.GetAll();
        }

        public QuoteRequest GetSingleQuote(int id)
        {
            return Repository.GetAll().Include(q => q.Category).FirstOrDefault(q => q.QuoteRequestId == id);
        }

        private IRepository<QuoteRequest> _repository;
        public IRepository<QuoteRequest> Repository
        {
            get
            {
                if(_repository == null)
                {
                    _repository = new Repository<QuoteRequest>(UserName);
                }
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }
    }
}