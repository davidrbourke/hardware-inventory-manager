using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HardwareInventoryManager.Helpers.User
{
    public interface IUserService
    {
        IQueryable<Models.ApplicationUser> GetUsers();
        Models.ApplicationUser GetUserById(string id);
        ApplicationUser GetUserByEmail(string email);
        ApplicationUser EditUser(ApplicationUser user);
        EnumHelper.Roles GetCurrentUserRoleById(string userId);
        ApplicationUser CreateUser(ApplicationUser user);
        void UpdateUserTenants();
        string[] Errors { get; set; }
    }
}
