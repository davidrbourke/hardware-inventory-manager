using HardwareInventoryManager.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Helpers.ApplicationSettings
{
    public interface IApplicationSettingsService
    {
        string GetEmailServiceUsername();
        string GetEmailServiceKeyCode();
        string GetEmailServiceSenderEmailAddress();
        EnumHelper.EmailServiceTypes GetEmailServiceOnlineType();
        void UpdateMultipleSettings(List<Models.ApplicationSetting> settings);
    }
}