using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services
{
    public class PermissionHelper
    {
        private IQueryable<RolePermission> _rolePermissions { get; set; }

        public PermissionHelper(IQueryable<RolePermission> rolePermissions)
        {
            _rolePermissions = rolePermissions;
        }

        /// <summary>
        /// Returns true if the ROLE has permission to the controller and action
        /// </summary>
        /// <param name="role"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool HasPermission(EnumHelper.Roles role, string controller, string action)
        {

            if (role == EnumHelper.Roles.Admin)
                return true;

            if(_rolePermissions.Any(r => 
                r.Role.Equals(role.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                r.Controller.Equals(controller, StringComparison.CurrentCultureIgnoreCase) && 
                r.Action.Equals(action, StringComparison.CurrentCultureIgnoreCase)))
            {
                return true;
            }
            return false;
        }
    }
}