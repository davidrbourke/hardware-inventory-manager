using HardwareInventoryManager.Services;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Services.ApplicationSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services.Messaging
{
    /// <summary>
    /// Determine which class should be returned for sending an email
    /// </summary>
    public class SendEmailFactory
    {
        public static SendEmailTemplate GetSendEmailType(IRepository<Email> emailRepository)
        {
            IApplicationSettingsService applicationSettings = new ApplicationSettingsService();
            switch (applicationSettings.GetEmailServiceOnlineType())
        	{
		        case EnumHelper.EmailServiceTypes.Offline:
                    return new SendEmailOffline(emailRepository);
                case EnumHelper.EmailServiceTypes.OnlineSendGrid:
                    return new SendEmailSendGrid(emailRepository);
                default:
                    return new SendEmailOffline(emailRepository);
            }
        }
    }
}