using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using SendGrid;

namespace HardwareInventoryManager.Services.Messaging
{
    public class CustomEmailService : IEmailService
    {
        public void SendEmail(string recipient, string subject, string body)
        {
            var message = new SendGridMessage();
            message.From = new MailAddress("david@solidcsharp.com");

            IList<string> recipients = new List<string>
            {
                recipient
            };

            message.AddTo(recipients);

            message.Subject = subject;

            message.Html = body;
            message.Text = body;

        }

        private void Deliver(SendGridMessage message)
        {
            var username = string.Empty;
            var password = string.Empty;

            var credentials = new NetworkCredential(username, password);

            var transportWeb = new Web(credentials);

            transportWeb.DeliverAsync(message);

        }


        public int TenantId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}