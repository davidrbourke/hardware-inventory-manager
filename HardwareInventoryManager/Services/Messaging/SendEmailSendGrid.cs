using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using SendGrid;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Helpers.ApplicationSettings;

namespace HardwareInventoryManager.Helpers.Messaging
{
    /// <summary>
    /// Uses the Sendgrid services to send a real email
    /// </summary>
    public class SendEmailSendGrid : SendEmailTemplate
    {
        private string _userName;
        private IRepository<Email> _repository;

        public SendEmailSendGrid(IRepository<Email> repository, string userName)
        {
            _repository = repository;
            _userName = userName;
        }

        protected override void SendEmail(string sender, string recipient, string subject, string body)
        {
            var message = new SendGridMessage();
            message.From = new MailAddress(sender);
            IList<string> recipients = new List<string>
            {
                recipient
            };
            message.AddTo(recipients);
            message.Subject = subject;
            message.Html = body;
            message.Text = body;
            Deliver(message);
        }

        private void Deliver(SendGridMessage message)
        {
            IApplicationSettingsService applicationSettings = new ApplicationSettingsService(_userName);
            var username = applicationSettings.GetEmailServiceUsername();
            var password = applicationSettings.GetEmailServiceKeyCode();
            var credentials = new NetworkCredential(username, password);
            var transportWeb = new Web(credentials);
            transportWeb.DeliverAsync(message);
        }

        protected override void LogEmail(string sender, string recipient, string subject, string body)
        {
            Email email = new Email
            {
                SenderEmailAddress = sender,
                RecipientsEmailAddress = recipient,
                Subject = subject,
                Body = body,
                TenantId = TenantId
            };
            _repository.Create(email);
            _repository.Save();
        }
    }
}