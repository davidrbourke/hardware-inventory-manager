using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Helpers
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

            // Is permission explicitely granted
            if(_rolePermissions.Any(r => 
                r.Role.Equals(role.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                r.Controller.Equals(controller, StringComparison.CurrentCultureIgnoreCase) && 
                r.Action.Equals(action, StringComparison.CurrentCultureIgnoreCase) &&
                r.IsAllowed))
            {
                return true;
            }
            
            // Is permission explicitly denied
            if (_rolePermissions.Any(r =>
                r.Role.Equals(role.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                r.Controller.Equals(controller, StringComparison.CurrentCultureIgnoreCase) &&
                r.Action.Equals(action, StringComparison.CurrentCultureIgnoreCase) &&
                !r.IsAllowed))
            {
                return false;
            }

            // Is permission explicitly denied for Role to full controller
            if (_rolePermissions.Any(r =>
                r.Role.Equals(role.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                r.Controller.Equals(controller, StringComparison.CurrentCultureIgnoreCase) &&
                (r.Action ==null || r.Action.Trim() == string.Empty) &&
                !r.IsAllowed))
            {
                return false;
            }


            // Revert to default permission using Action keywords
            // return false if user is a Viewer and wants to access Edit or Delete actions
            if(role == EnumHelper.Roles.Viewer && (action.Equals("Edit") || action.Equals("Delete")))
            {
                return false;
            } 
            else
            {
                return true;
            }
        }
    }
}