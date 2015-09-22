using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Helpers.ApplicationSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Helpers.Messaging
{
    /// <summary>
    /// Determine which class should be returned for sending an email
    /// </summary>
    public class SendEmailFactory
    {
        public static SendEmailTemplate GetSendEmailType(IRepository<Email> emailRepository, string userName)
        {
            IApplicationSettingsService applicationSettings = new ApplicationSettingsService(userName);
            switch (applicationSettings.GetEmailServiceOnlineType())
        	{
		        case EnumHelper.EmailServiceTypes.Offline:
                    return new SendEmailOffline(emailRepository);
                case EnumHelper.EmailServiceTypes.OnlineSendGrid:
                    return new SendEmailSendGrid(emailRepository, userName);
                default:
                    return new SendEmailOffline(emailRepository);
            }
        }
    }
}