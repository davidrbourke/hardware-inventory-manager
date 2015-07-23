using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardwareInventoryManager.Services;
using HardwareInventoryManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace HardwareInventoryManager.Tests.Services
{
    [TestClass]
    public class PermissionHelperTests
    {
        [TestMethod]
        public void HasPermission_Admin_True()
        {
            // ARRANGE
            List<RolePermission> rolePermissions = new List<RolePermission>
            {
                new RolePermission
                {
                    Controller = "LookupTypes",
                    Action = "Index",
                    Role = "Admin"
                },
                new RolePermission
                {
                    Controller = "LookupTypes",
                    Action = "Detail",
                    Role = "Admin"
                },
            };
            PermissionHelper permissionHelper = new PermissionHelper(rolePermissions.AsQueryable());
            
            // ACT
            bool hasPermission = permissionHelper.HasPermission(EnumHelper.Roles.Admin, "LookupTypes", "Index");

            // ASSERT
            Assert.IsTrue(hasPermission);
        }

        [TestMethod]
        public void HasPermission_Viewer_False()
        {
            // ARRANGE
            List<RolePermission> rolePermissions = new List<RolePermission>
            {
                new RolePermission
                {
                    Controller = "LookupTypes",
                    Action = "Index",
                    Role = "Author"
                },
                new RolePermission
                {
                    Controller = "LookupTypes",
                    Action = "Detail",
                    Role = "Viewer"
                },
            };
            PermissionHelper permissionHelper = new PermissionHelper(rolePermissions.AsQueryable());

            // ACT
            bool hasPermission = permissionHelper.HasPermission(EnumHelper.Roles.Viewer, "LookupTypes", "Index");

            // ASSERT
            Assert.IsFalse(hasPermission);
        }
    }
}
