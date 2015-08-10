using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareInventoryManager.Models;

namespace HardwareInventoryManager.Helpers.User
{
    public interface IUserService
    {
        IQueryable<ApplicationUser> GetUsers(int tenantId);
        ApplicationUser GetUser(int tenantId, string id);
        ApplicationUser EditUser(int tenantId, ApplicationUser user);
    }
}
