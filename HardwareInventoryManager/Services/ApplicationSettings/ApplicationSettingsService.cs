using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using HardwareInventoryManager.Helpers.User;
using HardwareInventoryManager.Services;

namespace HardwareInventoryManager.Helpers.ApplicationSettings
{
    /// <summary>
    /// Application Settings Service
    /// TODO: Replace methods here with connection to the database, currently this is
    /// only a stub.
    /// </summary>
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        private string _userId;
        
        public ApplicationSettingsService(string userId)
        {
            _userId = userId;
            CheckSeededData();
        }

        public void CheckSeededData()
        {
            SeedUserSettings seedUserSettings = new SeedUserSettings(_userId);
        }

        private IRepository<ApplicationSetting> _repository;
        public IRepository<ApplicationSetting> Repository
        {
            get
            {
                if(_repository == null)
                {
                    _repository = new RepositoryWithoutTenant<ApplicationSetting>() as IRepository<ApplicationSetting>;
                }
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }

        string IApplicationSettingsService.GetEmailServiceUsername()
        {
            ApplicationSetting setting = Repository.Single(x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.EmailServiceUserName.ToString());
            return setting != null ? setting.Value : string.Empty;
        }

        string IApplicationSettingsService.GetEmailServiceKeyCode()
        {
            ApplicationSetting setting = Repository.Single(x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.EmailServiceKeyCode.ToString());
            return setting != null ? setting.Value : string.Empty;
        }

        string IApplicationSettingsService.GetEmailServiceSenderEmailAddress()
        {
            ApplicationSetting setting = Repository.Single(x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.EmailServiceSenderEmailAddress.ToString());
            return setting != null ? setting.Value : string.Empty;
        }

        EnumHelper.EmailServiceTypes IApplicationSettingsService.GetEmailServiceOnlineType()
        {
            ApplicationSetting setting = Repository.Single(x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.EmailServiceOnlineType.ToString());
            if(setting == null)
                return EnumHelper.EmailServiceTypes.Offline;

            if (setting.Value == "false")
                return EnumHelper.EmailServiceTypes.Offline;
            else
                return EnumHelper.EmailServiceTypes.OnlineSendGrid;
        }

        /// <summary>
        /// Update multiple settings in one method
        /// </summary>
        /// <param name="settings"></param>
        public void UpdateMultipleSettings(List<ApplicationSetting> settings)
        {
            foreach(ApplicationSetting setting in settings)
            {
                if (setting.AppSetting.DataType == EnumHelper.AppSettingDataType.SecureString &&
                    string.IsNullOrWhiteSpace(setting.Value))
                {
                    
                } else
                {
                    Repository.Edit(setting);
                }
            }
            Repository.Save();
        }


        public IQueryable<ApplicationSetting> GetDashboardSettingsForUser()
        {
            return Repository.Find(
                x => x.ScopeType == EnumHelper.AppSettingScopeType.User &&
                x.UserId == _userId &&
                x.AppSetting.SettingType == EnumHelper.ApplicationSettingType.Dashboard)
                .Include(x => x.AppSetting);
        }
    }
}