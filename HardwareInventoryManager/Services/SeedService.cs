using HardwareInventoryManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services
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