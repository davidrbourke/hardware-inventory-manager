using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services.Messaging
{
    /// <summary>
    /// Abstract template for sending an email, includes sending and logging email
    /// </summary>
    public abstract class SendEmailTemplate
    {
        public void PrepareEmail(string sender, string recipient, string subject, string body)
        {
            SendEmail(sender, recipient, subject, body);
            LogEmail(sender, recipient, subject, body);
        }

        public int TenantId { get; set; }

        protected abstract void SendEmail(string sender, string recipient, string subject, string body);

        protected abstract void LogEmail(string sender, string recipient, string subject, string body);
    }
}