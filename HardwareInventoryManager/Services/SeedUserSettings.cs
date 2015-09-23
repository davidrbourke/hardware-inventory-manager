using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Helpers.User;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services
{
    public class SeedUserSettings
    {
        private CustomApplicationDbContext _context;
        private string _userId;

        public SeedUserSettings(string userId)
        {
            _userId = userId;
            _context = new CustomApplicationDbContext();
            SeedBasicApplicationSettings();
        }

        private void SeedBasicApplicationSettings()
        {
            // Button panel
            if (!_context.ApplicationSettings.Any(
                x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.DashboardButtonsPanel.ToString() &&
                    x.UserId == _userId))
            {

                var setting = _context.Settings.FirstOrDefault(x => x.Key == EnumHelper.ApplicationSettingKeys.DashboardButtonsPanel.ToString());

                ApplicationSetting appSetting = new ApplicationSetting
                {
                    AppSetting = setting,
                    ScopeType = EnumHelper.AppSettingScopeType.User,
                    Value = "true",
                    UserId = _userId
                };
                _context.ApplicationSettings.Add(appSetting);
                _context.SaveChanges();
            }

            // Notifications Panel
            if (!_context.ApplicationSettings.Any(
               x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.DashboardNotificationsPanel.ToString() &&
                    x.UserId == _userId))
            {

                var setting = _context.Settings.FirstOrDefault(x => x.Key == EnumHelper.ApplicationSettingKeys.DashboardNotificationsPanel.ToString());

                ApplicationSetting appSetting = new ApplicationSetting
                {
                    AppSetting = setting,
                    ScopeType = EnumHelper.AppSettingScopeType.User,
                    Value = "true",
                    UserId = _userId
                };
                _context.ApplicationSettings.Add(appSetting);
                _context.SaveChanges();
            }

            // All asset pie char Panel
            if (!_context.ApplicationSettings.Any(
               x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.DashboardAssetsPieChartPanel.ToString() &&
                    x.UserId == _userId))
            {

                var setting = _context.Settings.FirstOrDefault(x => x.Key == EnumHelper.ApplicationSettingKeys.DashboardAssetsPieChartPanel.ToString());

                ApplicationSetting appSetting = new ApplicationSetting
                {
                    AppSetting = setting,
                    ScopeType = EnumHelper.AppSettingScopeType.User,
                    Value = "true",
                    UserId = _userId
                };
                _context.ApplicationSettings.Add(appSetting);
                _context.SaveChanges();
            }

            // Obsolete panel
            if (!_context.ApplicationSettings.Any(
               x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.DashboardAssetsObsoleteChartPanel.ToString() &&
                    x.UserId == _userId))
            {

                var setting = _context.Settings.FirstOrDefault(x => x.Key == EnumHelper.ApplicationSettingKeys.DashboardAssetsObsoleteChartPanel.ToString());

                ApplicationSetting appSetting = new ApplicationSetting
                {
                    AppSetting = setting,
                    ScopeType = EnumHelper.AppSettingScopeType.User,
                    Value = "true",
                    UserId = _userId
                };
                _context.ApplicationSettings.Add(appSetting);
                _context.SaveChanges();
            }

            // Warranty Panel
            if (!_context.ApplicationSettings.Any(
               x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.DashboardAssetsWarrantyExpiryChartPanel.ToString() &&
                    x.UserId == _userId))
            {

                var setting = _context.Settings.FirstOrDefault(x => x.Key == EnumHelper.ApplicationSettingKeys.DashboardAssetsWarrantyExpiryChartPanel.ToString());

                ApplicationSetting appSetting = new ApplicationSetting
                {
                    AppSetting = setting,
                    ScopeType = EnumHelper.AppSettingScopeType.User,
                    Value = "true",
                    UserId = _userId
                };
                _context.ApplicationSettings.Add(appSetting);
                _context.SaveChanges();
            }

            // Wishlist panel
            if (!_context.ApplicationSettings.Any(
               x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.DashboardAssetsWishlistStatsPanel.ToString() &&
                    x.UserId == _userId))
            {

                var setting = _context.Settings.FirstOrDefault(x => x.Key == EnumHelper.ApplicationSettingKeys.DashboardAssetsWishlistStatsPanel.ToString());

                ApplicationSetting appSetting = new ApplicationSetting
                {
                    AppSetting = setting,
                    ScopeType = EnumHelper.AppSettingScopeType.User,
                    Value = "true",
                    UserId = _userId
                };
                _context.ApplicationSettings.Add(appSetting);
                _context.SaveChanges();
            }
        }
    }
}