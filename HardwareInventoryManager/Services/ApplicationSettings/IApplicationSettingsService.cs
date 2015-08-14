using HardwareInventoryManager.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services.ApplicationSettings
{
    public interface IApplicationSettingsService
    {
        string GetEmailServiceUsername();
        string GetEmailServiceKeyCode();
        string GetEmailServiceSenderEmailAddress();
        EnumHelper.EmailServiceTypes GetEmailServiceOnlineType();
    }
}