using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Models;

namespace HardwareInventoryManager.Services.Messaging
{
    public class OfflineEmailService : IEmailService
    {
        private IRepository<Email> _repository;

        public OfflineEmailService(IRepository<Email> repository)
        {
            _repository = repository;
        }

        public int TenantId { get; set; }
        
        public void SendEmail(string sender, string recipient, string subject, string body)
        {
            Email email = new Email
            {
                SenderEmailAddress = sender,
                RecipientsEmailAddress = recipient,
                Subject = subject,
                Body = body,
                TenantContextId = TenantId
            };
            _repository.Create(email, TenantId);
            _repository.Save();
        }
    }
}