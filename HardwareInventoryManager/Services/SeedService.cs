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
                    TenantOrganisation = new Organisation
                    {
                        Name = "Orbital"
                    }
                };
                Tenant sampleTenantB = new Tenant
                {
                    TenantOrganisation = new Organisation
                    {
                        Name = "Polar"
                    }
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
                Tenant tenant = _context.Tenants.First(t => t.TenantOrganisation.Name == "Polar");
                if (tenant != null)
                {
                    var user = new ApplicationUser { UserName = emailAddress, Email = emailAddress, TenantId = tenant.TenantId };

                    roleManager.Create(new IdentityRole { Name = EnumHelper.Roles.Admin.ToString() });
                    manager.Create(user, "password");
                    manager.AddToRole(user.Id, EnumHelper.Roles.Admin.ToString());
                }
            }
        }

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
        /// Seed all the Lookups
        /// </summary>
        private void SeedLookups()
        {
            if (!_context.Lookups.Any())
            {
                SeedMakeLookup();
                SeedCategoryLookup();
                SeedWarrantyPeriodLookup();
            }
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

            string[] warrantyPeriods = { "3 Months", "1 Year", "2 Years", "3 Years" };
            foreach (string warrantyPeriod in warrantyPeriods)
            {
                Lookup initLookup = new Lookup
                {
                    Description = warrantyPeriod,
                    Type = warrantyPeriodType
                };
                _context.Lookups.Add(initLookup);
            }
            _context.SaveChanges();
        }
    }
}