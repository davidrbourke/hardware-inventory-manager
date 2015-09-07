using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Services.ApplicationSettings;
using HardwareInventoryManager.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HardwareInventoryManager.Services.Messaging
{
    public class ProcessEmail : IProcessEmail
    {
        private SendEmailTemplate _emailService;
        private ITenantUtility _tenantUtility;
        public string UserName { get; set; }

        public ProcessEmail(SendEmailTemplate emailService, ITenantUtility tenantUtility, string userName)
        {
            UserName = userName;
            _emailService = emailService;
            _tenantUtility = tenantUtility;
        }

        public ProcessEmail(string userName)        
        {
            UserName = userName;    
            _emailService = CreateSendEmailService();
            _tenantUtility = new TenantUtility();
        }

        public void SendEmail(string senderEmailAddress, string[] recipientsEmailAddresses, string subject, string body)
        {
            foreach (string recipientEmail in recipientsEmailAddresses)
            {
                _emailService.TenantId = _tenantUtility.GetTenantIdFromEmail(recipientEmail);
                _emailService.PrepareEmail(senderEmailAddress, recipientEmail, subject, body);
            }
        }

        public void SendPasswordResetEmail(ApplicationUser recipientUser, string callbackUrl)
        {
            EmailRepository.SetCurrentUser(recipientUser);
            string body = string.Format(HIResources.Strings.EmailBody_PasswordReset, callbackUrl);
            SendEmail(AdminEmailAddress(), new string[] { recipientUser.Email }, HIResources.Strings.EmailSubject_PasswordReset, body);    
        }

        public void SendEmailConfirmationEmail(ApplicationUser recipientUser, string callbackUrl)
        {
            EmailRepository.SetCurrentUser(recipientUser);
            string body = string.Format(HIResources.Strings.EmailBody_ConfirmUserEmail, callbackUrl);
            SendEmail(AdminEmailAddress(), new string[] { recipientUser.Email }, HIResources.Strings.EmailSubject_ConfirmUserEmail, body);    
        }

        public void SendNewAccountSetupEmail(ApplicationUser recipientUser)
        {
            EmailRepository.SetCurrentUser(recipientUser);
            string body = string.Format(HIResources.Strings.EmailBody_NewAccount, recipientUser.Email, recipientUser.TemporaryCode);
            SendEmail(AdminEmailAddress(), new string[] { recipientUser.Email }, HIResources.Strings.EmailSubject_NewAccount, body);    
        }

        private string AdminEmailAddress()
        {
            IApplicationSettingsService applicationSettings = new ApplicationSettingsService();
            return applicationSettings.GetEmailServiceSenderEmailAddress();
        }

        private SendEmailTemplate CreateSendEmailService()
        {
            return SendEmailFactory.GetSendEmailType(EmailRepository);
        }

        private IRepository<Email> _emailRepository;
        public IRepository<Email> EmailRepository
        {
            get
            {
                if(_emailRepository == null)
                {
                    _emailRepository = new Repository<Email>(UserName);
                }
                return _emailRepository;
            }
            set
            {
                _emailRepository = value;
            }
        }
    }
}