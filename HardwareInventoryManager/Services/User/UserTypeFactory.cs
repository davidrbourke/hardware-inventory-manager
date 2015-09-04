using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services.User
{
    public class UserTypeFactory
    {
        public static IUserService GetUserService(UserServiceUoW userServiceUoW)
        {
            switch (userServiceUoW.UserRole)
            {
                case EnumHelper.Roles.Admin:
                    return new AdminUserService(userServiceUoW.DbContext, userServiceUoW.TenantId);
                case EnumHelper.Roles.Author:
                case EnumHelper.Roles.Viewer:
                default:
                    return new UserService(userServiceUoW.DbContext, userServiceUoW.UserRepository);
            }
        }
    }
}