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
            SeedInitialUserAndRole();
            SeedLookupsAndTypes();
        }

        private void SeedInitialUserAndRole()
        {
            string emailAddress = "admin@admin.com";
            if (!_context.Users.Any(u => u.UserName == emailAddress))
            {
                var roleStore = new RoleStore<IdentityRole>(_context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var store = new UserStore<ApplicationUser>(_context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = emailAddress, Email = emailAddress };

                roleManager.Create(new IdentityRole { Name = "admin" });
                manager.Create(user, "password");
                manager.AddToRole(user.Id, "admin");
            }
        }

        private void SeedLookupsAndTypes()
        {
            if(!_context.Lookups.Any())
            {
                LookupType makeType = new LookupType
                {
                    Description = "Make"
                };

                _context.LookupTypes.Add(makeType);
                _context.SaveChanges();

                string[] makes = { "Dell", "Lenovo", "HP" };
                foreach(string make in makes)
                {
                    Lookup initLookup = new Lookup
                    {
                        Description = make,
                        Type = makeType
                    };
                    _context.Lookups.Add(initLookup);
                }
                _context.SaveChanges();


                LookupType categoryType = new LookupType
                {
                    Description = "Category"
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
        }
    }
}