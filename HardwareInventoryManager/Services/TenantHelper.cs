using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services
{
    public class TenantHelper
    {
        public static int LoadTenant(string userName)
        {
            CustomApplicationDbContext context = new CustomApplicationDbContext();
            ApplicationUser user = context.Users.First(u => u.UserName == userName);
            return user.TenantId;
        
        }
    }
}