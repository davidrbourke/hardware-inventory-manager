using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardwareInventoryManager.Services.User
{
    public interface IUserService
    {
        IQueryable<Models.ApplicationUser> GetUsers(int tenantId);
        Models.ApplicationUser GetUser(int tenantId, string id);
        ApplicationUser GetUserByEmail(int tenantId, string email);
        ApplicationUser EditUser(int tenantId, ApplicationUser user);
    }
}
