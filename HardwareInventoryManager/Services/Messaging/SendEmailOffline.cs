using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Models;

namespace HardwareInventoryManager.Services.Messaging
{
    /// <summary>
    /// This class does not send an email, it only logs the email to the database
    /// </summary>
    public class SendEmailOffline : SendEmailTemplate
    {
        private IRepository<Email> _repository;

        public SendEmailOffline(IRepository<Email> repository)
        {
            _repository = repository;
        }

        protected override void SendEmail(string sender, string recipient, string subject, string body)
        {
            // Offline - nothing to send to
        }

        protected override void LogEmail(string sender, string recipient, string subject, string body)
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