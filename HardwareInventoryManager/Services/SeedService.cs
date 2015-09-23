using HardwareInventoryManager.Models;
using HardwareInventoryManager.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Helpers
{
    public class SeedService
    {
        private CustomApplicationDbContext _context;

        public SeedService(CustomApplicationDbContext context)
        {
            _context = context;
            SeedLookups();
            SeedTenants();
            SeedInitialUserAndRole();
            SeedRoles();
            SeedDefaultRolePermissions();
            SeedApplicationSettings();
            SeedUserSettings();

            // Must come last
            SeedBasicApplicationSettings();
        }

        /// <summary>
        /// Seed app settings - user level types
        /// </summary>
        private void SeedUserSettings()
        {

            // Dashboard Buttons Panel
            if (!_context.Settings.Any(x =>
                x.Key.Equals(Helpers.EnumHelper.ApplicationSettingKeys.DashboardButtonsPanel.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)))
            {
                Setting appSetting = new Setting
                {
                    Key = Helpers.EnumHelper.ApplicationSettingKeys.DashboardButtonsPanel.ToString(),
                    DataType = EnumHelper.AppSettingDataType.Bool,
                    //ScopeType = EnumHelper.AppSettingScopeType.User,
                    SettingType = EnumHelper.ApplicationSettingType.Dashboard,
                    //Value = "true"
                };
                _context.Settings.Add(appSetting);
                _context.SaveChanges();
            }

            // Dashboard Notifications Panel
            if (!_context.Settings.Any(x =>
                x.Key.Equals(Helpers.EnumHelper.ApplicationSettingKeys.DashboardNotificationsPanel.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)))
            {
                Setting appSetting = new Setting
                {
                    Key = Helpers.EnumHelper.ApplicationSettingKeys.DashboardNotificationsPanel.ToString(),
                    DataType = EnumHelper.AppSettingDataType.Bool,
                    //ScopeType = EnumHelper.AppSettingScopeType.User,
                    SettingType = EnumHelper.ApplicationSettingType.Dashboard,
                    //Value = "true"
                };
                _context.Settings.Add(appSetting);
                _context.SaveChanges();
            }

            // Dashboard All Assets Panel
            if (!_context.Settings.Any(x =>
                x.Key.Equals(Helpers.EnumHelper.ApplicationSettingKeys.DashboardAssetsPieChartPanel.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)))
            {
                Setting appSetting = new Setting
                {
                    Key = Helpers.EnumHelper.ApplicationSettingKeys.DashboardAssetsPieChartPanel.ToString(),
                    DataType = EnumHelper.AppSettingDataType.Bool,
                    //ScopeType = EnumHelper.AppSettingScopeType.User,
                    SettingType = EnumHelper.ApplicationSettingType.Dashboard,
                    //Value = "true"
                };
                _context.Settings.Add(appSetting);
                _context.SaveChanges();
            }


            // Dashboard All Assets Obsolete Panel
            if (!_context.Settings.Any(x =>
                x.Key.Equals(Helpers.EnumHelper.ApplicationSettingKeys.DashboardAssetsObsoleteChartPanel.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)))
            {
                Setting appSetting = new Setting
                {
                    Key = Helpers.EnumHelper.ApplicationSettingKeys.DashboardAssetsObsoleteChartPanel.ToString(),
                    DataType = EnumHelper.AppSettingDataType.Bool,
                    //ScopeType = EnumHelper.AppSettingScopeType.User,
                    SettingType = EnumHelper.ApplicationSettingType.Dashboard,
                    //Value = "true"
                };
                _context.Settings.Add(appSetting);
                _context.SaveChanges();
            }

            // Dashboard WarrantyExpiry Obsolete Panel
            if (!_context.Settings.Any(x =>
                x.Key.Equals(Helpers.EnumHelper.ApplicationSettingKeys.DashboardAssetsWarrantyExpiryChartPanel.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)))
            {
                Setting appSetting = new Setting
                {
                    Key = Helpers.EnumHelper.ApplicationSettingKeys.DashboardAssetsWarrantyExpiryChartPanel.ToString(),
                    DataType = EnumHelper.AppSettingDataType.Bool,
                    //ScopeType = EnumHelper.AppSettingScopeType.User,
                    SettingType = EnumHelper.ApplicationSettingType.Dashboard,
                    //Value = "true"
                };
                _context.Settings.Add(appSetting);
                _context.SaveChanges();
            }

            // Dashboard Wishlist Panel
            if (!_context.Settings.Any(x =>
                x.Key.Equals(Helpers.EnumHelper.ApplicationSettingKeys.DashboardAssetsWishlistStatsPanel.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)))
            {
                Setting appSetting = new Setting
                {
                    Key = Helpers.EnumHelper.ApplicationSettingKeys.DashboardAssetsWishlistStatsPanel.ToString(),
                    DataType = EnumHelper.AppSettingDataType.Bool,
                    //ScopeType = EnumHelper.AppSettingScopeType.User,
                    SettingType = EnumHelper.ApplicationSettingType.Dashboard,
                    //Value = "true"
                };
                _context.Settings.Add(appSetting);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Seed app settings - global types
        /// </summary>
        private void SeedApplicationSettings()
        {
            // Email Service User Name
            if(!_context.Settings.Any(x => 
                x.Key.Equals(Helpers.EnumHelper.ApplicationSettingKeys.EmailServiceUserName.ToString(), 
                    StringComparison.CurrentCultureIgnoreCase)))
            {
                Setting appSetting = new Setting
                {
                    Key = Helpers.EnumHelper.ApplicationSettingKeys.EmailServiceUserName.ToString(),
                    DataType = EnumHelper.AppSettingDataType.String,
                    //ScopeType = EnumHelper.AppSettingScopeType.Application,
                    SettingType = EnumHelper.ApplicationSettingType.Email
                };
                _context.Settings.Add(appSetting);
                _context.SaveChanges();
            }

            // Email Service Key Code
            if (!_context.Settings.Any(x =>
                x.Key.Equals(Helpers.EnumHelper.ApplicationSettingKeys.EmailServiceKeyCode.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)))
            {
                Setting appSetting = new Setting
                {
                    Key = Helpers.EnumHelper.ApplicationSettingKeys.EmailServiceKeyCode.ToString(),
                    DataType = EnumHelper.AppSettingDataType.SecureString,
                    //ScopeType = EnumHelper.AppSettingScopeType.Application,
                    SettingType = EnumHelper.ApplicationSettingType.Email
                };
                _context.Settings.Add(appSetting);
                _context.SaveChanges();
            }

            // Email Service Sender Email Address
            if (!_context.Settings.Any(x =>
                x.Key.Equals(Helpers.EnumHelper.ApplicationSettingKeys.EmailServiceSenderEmailAddress.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)))
            {
                Setting appSetting = new Setting
                {
                    Key = Helpers.EnumHelper.ApplicationSettingKeys.EmailServiceSenderEmailAddress.ToString(),
                    DataType = EnumHelper.AppSettingDataType.String,
                    //ScopeType = EnumHelper.AppSettingScopeType.Application,
                    //Value = HIResources.Strings.DefaultAppEmailAddress,
                    SettingType = EnumHelper.ApplicationSettingType.Email
                };
                _context.Settings.Add(appSetting);
                _context.SaveChanges();
            }

            // Email Service Online Type
            if (!_context.Settings.Any(x =>
                x.Key.Equals(Helpers.EnumHelper.ApplicationSettingKeys.EmailServiceOnlineType.ToString(),
                    StringComparison.CurrentCultureIgnoreCase)))
            {
                Setting appSetting = new Setting
                {
                    Key = Helpers.EnumHelper.ApplicationSettingKeys.EmailServiceOnlineType.ToString(),
                    DataType = EnumHelper.AppSettingDataType.Bool,
                    //ScopeType = EnumHelper.AppSettingScopeType.Application,
                    //Value = "false",
                    SettingType = EnumHelper.ApplicationSettingType.Email
                };
                _context.Settings.Add(appSetting);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Seed app settings for a for the applications - general - only a single instance of these
        /// settings for the entire application
        /// </summary>
        private void SeedBasicApplicationSettings()
        {
            if(!_context.ApplicationSettings.Any(
                x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.EmailServiceUserName.ToString()))
            {

                var setting = _context.Settings.FirstOrDefault(x => x.Key == EnumHelper.ApplicationSettingKeys.EmailServiceUserName.ToString());

                ApplicationSetting appSetting = new ApplicationSetting
                {
                    AppSetting = setting,
                    ScopeType = EnumHelper.AppSettingScopeType.Application
                };
                _context.ApplicationSettings.Add(appSetting);
                _context.SaveChanges();
            }

            if (!_context.ApplicationSettings.Any(
                x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.EmailServiceKeyCode.ToString()))
            {

                var setting = _context.Settings.FirstOrDefault(x => x.Key == EnumHelper.ApplicationSettingKeys.EmailServiceKeyCode.ToString());

                ApplicationSetting appSetting = new ApplicationSetting
                {
                    AppSetting = setting,
                    ScopeType = EnumHelper.AppSettingScopeType.Application
                };
                _context.ApplicationSettings.Add(appSetting);
                _context.SaveChanges();
            }

            if (!_context.ApplicationSettings.Any(
                x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.EmailServiceSenderEmailAddress.ToString()))
            {

                var setting = _context.Settings.FirstOrDefault(x => x.Key == EnumHelper.ApplicationSettingKeys.EmailServiceSenderEmailAddress.ToString());

                ApplicationSetting appSetting = new ApplicationSetting
                {
                    AppSetting = setting,
                    ScopeType = EnumHelper.AppSettingScopeType.Application,
                    Value = HIResources.Strings.DefaultAppEmailAddress
                };
                _context.ApplicationSettings.Add(appSetting);
                _context.SaveChanges();
            }

            if (!_context.ApplicationSettings.Any(
                x => x.AppSetting.Key == EnumHelper.ApplicationSettingKeys.EmailServiceOnlineType.ToString()))
            {

                var setting = _context.Settings.FirstOrDefault(x => x.Key == EnumHelper.ApplicationSettingKeys.EmailServiceOnlineType.ToString());

                ApplicationSetting appSetting = new ApplicationSetting
                {
                    AppSetting = setting,
                    ScopeType = EnumHelper.AppSettingScopeType.Application
                };
                _context.ApplicationSettings.Add(appSetting);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Seed the initial sample tenants
        /// </summary>
        private void SeedTenants()
        {
            if (!_context.Tenants.Any())
            {
                Tenant sampleTenantA = new Tenant
                {
                    Name = "Orbital"
                };
                Tenant sampleTenantB = new Tenant
                {
                    Name = "Polar"
                };
                _context.Tenants.Add(sampleTenantA);
                _context.Tenants.Add(sampleTenantB);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Seed an initial Admin user
        /// </summary>
        private void SeedInitialUserAndRole()
        {
            string emailAddress = "admin@admin.com";
            if (!_context.Users.Any(u => u.UserName == emailAddress))
            {
                var roleStore = new RoleStore<IdentityRole>(_context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var store = new UserStore<ApplicationUser>(_context);
                var manager = new UserManager<ApplicationUser>(store);
                Tenant tenant = _context.Tenants.First(t => t.Name == "Polar");
                if (tenant != null)
                {
                    IList<Tenant> tenants = new List<Tenant>();
                    tenants.Add(tenant);
                    var user = new ApplicationUser { UserName = emailAddress, Email = emailAddress, UserTenants = tenants };

                    roleManager.Create(new IdentityRole { Name = EnumHelper.Roles.Admin.ToString() });
                    manager.Create(user, "password");
                    manager.AddToRole(user.Id, EnumHelper.Roles.Admin.ToString());

                    SeedUserSettings seedUserSettings = new SeedUserSettings(user.Id);
                    
                }
            }
        }

        /// <summary>
        /// Seed inital Roles
        /// </summary>
        private void SeedRoles()
        {
            if (!_context.Roles.Any(x => x.Name == EnumHelper.Roles.Author.ToString()))
            {
                var roleStore = new RoleStore<IdentityRole>(_context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                roleManager.Create(new IdentityRole { Name = EnumHelper.Roles.Author.ToString() });
            }
            if (!_context.Roles.Any(x => x.Name == EnumHelper.Roles.Viewer.ToString()))
            {
                var roleStore = new RoleStore<IdentityRole>(_context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                roleManager.Create(new IdentityRole { Name = EnumHelper.Roles.Viewer.ToString() });
            }
        }

        /// <summary>
        /// Seed default Roles
        /// </summary>
        private void SeedDefaultRolePermissions()
        {
            if(!_context.RolePermissions.Any(r => r.Role.Equals(EnumHelper.Roles.Author.ToString()) &&
                r.Controller.Equals("LookupTypes")))
            {
                RolePermission authorRolePermission = new RolePermission
                {
                    Role = EnumHelper.Roles.Author.ToString(),
                    Controller = "LookupTypes",
                    IsAllowed = false,
                    CreatedDate = DateTime.Now
                };

                _context.RolePermissions.Add(authorRolePermission);
                _context.SaveChanges();
            }

            if (!_context.RolePermissions.Any(r => r.Role.Equals(EnumHelper.Roles.Viewer.ToString()) &&
                r.Controller.Equals("LookupTypes")))
            {
                RolePermission authorRolePermission = new RolePermission
                {
                    Role = EnumHelper.Roles.Viewer.ToString(),
                    Controller = "LookupTypes",
                    IsAllowed = false,
                    CreatedDate = DateTime.Now
                };

                _context.RolePermissions.Add(authorRolePermission);
                _context.SaveChanges();
            }

            if (!_context.RolePermissions.Any(r => r.Role.Equals(EnumHelper.Roles.Author.ToString()) &&
                r.Controller.Equals("Tenants")))
            {
                RolePermission authorRolePermission = new RolePermission
                {
                    Role = EnumHelper.Roles.Author.ToString(),
                    Controller = "Tenants",
                    IsAllowed = false,
                    CreatedDate = DateTime.Now
                };

                _context.RolePermissions.Add(authorRolePermission);
                _context.SaveChanges();
            }

            if (!_context.RolePermissions.Any(r => r.Role.Equals(EnumHelper.Roles.Viewer.ToString()) &&
                r.Controller.Equals("Tenants")))
            {
                RolePermission authorRolePermission = new RolePermission
                {
                    Role = EnumHelper.Roles.Viewer.ToString(),
                    Controller = "Tenants",
                    IsAllowed = false,
                    CreatedDate = DateTime.Now
                };

                _context.RolePermissions.Add(authorRolePermission);
                _context.SaveChanges();
            }

            if (!_context.RolePermissions.Any(r => r.Role.Equals(EnumHelper.Roles.Author.ToString()) &&
                r.Controller.Equals("Contacts") &&
                r.Action.Equals("Index")))
            {
                RolePermission authorRolePermission = new RolePermission
                {
                    Role = EnumHelper.Roles.Author.ToString(),
                    Controller = "Contacts",
                    Action = "Index",
                    IsAllowed = false,
                    CreatedDate = DateTime.Now
                };

                _context.RolePermissions.Add(authorRolePermission);
                _context.SaveChanges();
            }

            if (!_context.RolePermissions.Any(r => r.Role.Equals(EnumHelper.Roles.Viewer.ToString()) &&
                r.Controller.Equals("Contacts") &&
                r.Action.Equals("Index")))
            {
                RolePermission authorRolePermission = new RolePermission
                {
                    Role = EnumHelper.Roles.Viewer.ToString(),
                    Controller = "Contacts",
                    Action = "Index",
                    IsAllowed = false,
                    CreatedDate = DateTime.Now
                };

                _context.RolePermissions.Add(authorRolePermission);
                _context.SaveChanges();
            }


            // Permissions for Application Settings - Admin only
            if (!_context.RolePermissions.Any(r => r.Role.Equals(EnumHelper.Roles.Author.ToString()) &&
                r.Controller.Equals("ApplicationSettings") &&
                r.Action.Equals("Index")))
            {
                RolePermission authorRolePermission = new RolePermission
                {
                    Role = EnumHelper.Roles.Author.ToString(),
                    Controller = "ApplicationSettings",
                    Action = "Index",
                    IsAllowed = false,
                    CreatedDate = DateTime.Now
                };

                _context.RolePermissions.Add(authorRolePermission);
                _context.SaveChanges();
            }

            if (!_context.RolePermissions.Any(r => r.Role.Equals(EnumHelper.Roles.Viewer.ToString()) &&
                r.Controller.Equals("ApplicationSettings") &&
                r.Action.Equals("Index")))
            {
                RolePermission authorRolePermission = new RolePermission
                {
                    Role = EnumHelper.Roles.Viewer.ToString(),
                    Controller = "ApplicationSettings",
                    Action = "Index",
                    IsAllowed = false,
                    CreatedDate = DateTime.Now
                };

                _context.RolePermissions.Add(authorRolePermission);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Seed all the Lookups
        /// </summary>
        private void SeedLookups()
        {
            if (!_context.Lookups.Any())
            {
                SeedMakeLookup();
                SeedCategoryLookup();    
            }
            if(!_context.LookupTypes.Any(x=>x.Description == EnumHelper.LookupTypes.WarrantyPeriod.ToString()))
            {
                SeedWarrantyPeriodLookup();
            }
            if(!_context.LookupTypes.Any(x => x.Description == EnumHelper.LookupTypes.QuoteRequestStatus.ToString()))
            {
                SeedQuoteRequestStatus();
            }
        }

        /// <summary>
        /// Seed the Request Status Lookup for QuoteRequests
        /// </summary>
        private void SeedQuoteRequestStatus()
        {
            LookupType quoteRequestStatus = new LookupType
            {
                Description = EnumHelper.LookupTypes.QuoteRequestStatus.ToString()
            };

            _context.LookupTypes.Add(quoteRequestStatus);
            _context.SaveChanges();

            string[] quoteRequestStatusTypes = { 
                                                   EnumHelper.QuoteRequestTypes.Pending.ToString(),
                                                   EnumHelper.QuoteRequestTypes.Processing.ToString(),
                                                   EnumHelper.QuoteRequestTypes.Supplied.ToString(),
                                                   EnumHelper.QuoteRequestTypes.Complete.ToString()
                                               };
            foreach(string quoteRequestStatusType in quoteRequestStatusTypes)
            {
                Lookup quoteRequestStatusLookup = new Lookup
                {
                    Description = quoteRequestStatusType,
                    Type = quoteRequestStatus
                };
                _context.Lookups.Add(quoteRequestStatusLookup);
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Seed the Make Lookup for Assets
        /// </summary>
        public void SeedMakeLookup()
        {
            LookupType makeType = new LookupType
            {
                Description = EnumHelper.LookupTypes.Make.ToString()
            };

            _context.LookupTypes.Add(makeType);
            _context.SaveChanges();

            string[] makes = { "Dell", "Lenovo", "HP" };
            foreach (string make in makes)
            {
                Lookup initLookup = new Lookup
                {
                    Description = make,
                    Type = makeType
                };
                _context.Lookups.Add(initLookup);
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Seed the Category Lookup for Assets
        /// </summary>
        public void SeedCategoryLookup()
        {
            LookupType categoryType = new LookupType
            {
                Description = EnumHelper.LookupTypes.Category.ToString()
            };

            _context.LookupTypes.Add(categoryType);
            _context.SaveChanges();

            string[] categories = { "Desktop", "Notebook", "Server", "Tablet" };
            foreach (string category in categories)
            {
                Lookup initLookup = new Lookup
                {
                    Description = category,
                    Type = categoryType
                };
                _context.Lookups.Add(initLookup);
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Seed the Warranty Period Lookup for Assets
        /// </summary>
        public void SeedWarrantyPeriodLookup()
        {
            LookupType warrantyPeriodType = new LookupType
            {
                Description = EnumHelper.LookupTypes.WarrantyPeriod.ToString()
            };

            _context.LookupTypes.Add(warrantyPeriodType);
            _context.SaveChanges();

            string[] warrantyPeriods = { "1 Year", "2 Years", "3 Years", "4 Years", "5 Years", "6 Years" };
            foreach (string warrantyPeriod in warrantyPeriods)
            {
                Lookup initLookup = new Lookup
                {
                    Description = warrantyPeriod,
                    Type = warrantyPeriodType,
                    AssociatedNumericValue = int.Parse(warrantyPeriod.Substring(0, 1))
                };
                _context.Lookups.Add(initLookup);
            }
            _context.SaveChanges();
        }
    }
}