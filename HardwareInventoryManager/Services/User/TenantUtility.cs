using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HardwareInventoryManager.Services.User;

namespace HardwareInventoryManager.Services.User
{
    public class TenantUtility : ITenantUtility
    {
        private CustomApplicationDbContext _context;

        public TenantUtility()
        {
            _context = new CustomApplicationDbContext();
        }

        public int GetTenantIdFromEmail(string emailAddress)
        {
            int tenantContextId = 0;
            
                ApplicationUser uu = _context.Users.Include(x => x.UserTenants).First(u => u.UserName == emailAddress) as ApplicationUser;
                if (uu.UserTenants != null && uu.UserTenants.Count > 0)
                {
                    return uu.UserTenants.First().TenantId;
                }
            return tenantContextId;
        }

        public int GetTenantIdFromUserId(string userId)
        {
            int tenantContextId = 0;
            ApplicationUser uu = _context.Users.Include(x => x.UserTenants).First(u => u.Id == userId) as ApplicationUser;
            if (uu.UserTenants != null && uu.UserTenants.Count > 0)
            {
                return uu.UserTenants.First().TenantId;
            }
            return tenantContextId;
        }

        public IQueryable<Tenant> GetUserTenants(string userName)
        {
            ApplicationUser uu = _context.Users.Include(x => x.UserTenants).First(u => u.UserName == userName) as ApplicationUser;
            if (uu.UserTenants != null && uu.UserTenants.Count > 0)
            {
                return uu.UserTenants.AsQueryable();
            }
            return new List<Tenant>().AsQueryable();
        }

    }
}