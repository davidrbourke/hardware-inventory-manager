using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services.User
{
    public class UserTypeFactory
    {
        public static IUserService GetUserService(EnumHelper.Roles role, CustomApplicationDbContext context)
        {
            switch (role)
            {
                case EnumHelper.Roles.Admin:
                    return new AdminUserService(context);
                case EnumHelper.Roles.Author:
                case EnumHelper.Roles.Viewer:
                default:
                    return new UserService(context);
            }
        }
    }
}