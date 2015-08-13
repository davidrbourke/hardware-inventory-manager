using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
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
        private IRepository<Email> _emailRepository;
        private IEmailService _emailService;
        private IUserUtility _userUtility;
        private ITenantUtility _tenantUtility;

        public ProcessEmail(IEmailService emailService, IUserUtility userUtility, ITenantUtility tenantUtility)
        {
            _emailService = emailService;
            _userUtility = userUtility;
            _tenantUtility = tenantUtility;
        }

        public ProcessEmail()
        {
            _emailRepository = new Repository<Email>();
            _emailService = new OfflineEmailService(_emailRepository);
            _userUtility = new UserUtility();
            _tenantUtility = new TenantUtility();
        }

        public void SendEmail(string senderEmailAddress, string[] recipientsEmailAddresses, string subject, string body)
        {
            //IEmailService emailService = new OfflineEmailService(repository);
            foreach (string recipientEmail in recipientsEmailAddresses)
            {
                _emailService.TenantId = _tenantUtility.GetTenantIdFromEmail(recipientEmail);
                _emailService.SendEmail(senderEmailAddress, recipientEmail, subject, body);
            }
        }


        public void SendPasswordResetEmail(ApplicationUser recipientUser, string callbackUrl)
        {
            recipientUser = _userUtility.GetUserById(recipientUser.Id);
            _emailRepository.SetCurrentUser(recipientUser);
            string body = string.Format(HIResources.Strings.EmailBody_PasswordReset, callbackUrl);
            SendEmail(AdminEmailAddress(), new string[] { recipientUser.Email }, HIResources.Strings.EmailSubject_PasswordReset, body);    
        }

        public void SendEmailConfirmationEmail(ApplicationUser recipientUser, string callbackUrl)
        {
            recipientUser = _userUtility.GetUserById(recipientUser.Id);
            _emailRepository.SetCurrentUser(recipientUser);
            string body = string.Format(HIResources.Strings.EmailBody_ConfirmUserEmail, callbackUrl);
            SendEmail(AdminEmailAddress(), new string[] { recipientUser.Email }, HIResources.Strings.EmailSubject_ConfirmUserEmail, body);    
        }

        public void SendNewAccountSetupEmail(string recipient, ApplicationUserManager applicationUserManager)
        {
            throw new NotImplementedException();
        }

        private string AdminEmailAddress()
        {
            // TODO: Return from database
            return "david@drbtechnology.com";
        }
    }
}