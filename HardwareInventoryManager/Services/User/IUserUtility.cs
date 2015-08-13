using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services.User
{
    public interface IUserUtility
    {
        ApplicationUser GetUserById(string userId);
        ApplicationUser GetUserByEmail(string userEmail);
        EnumHelper.Roles GetCurrentUserRoleById(string userId);
    }
}