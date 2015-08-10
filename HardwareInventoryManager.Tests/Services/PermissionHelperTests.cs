using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardwareInventoryManager.Helpers;
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
        public void HasPermission_ViewerAccessingIndexNoPermissionDefined_True()
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
            Assert.IsTrue(hasPermission);
        }

        [TestMethod]
        public void HasPermission_ViewerAccessingDeleteNoPermissionDefined_False()
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
            bool hasPermission = permissionHelper.HasPermission(EnumHelper.Roles.Viewer, "Assets", "Delete");

            // ASSERT
            Assert.IsFalse(hasPermission);
        }

        [TestMethod]
        public void HasPermission_ViewerAccessingEditNoPermissionDefined_False()
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
            bool hasPermission = permissionHelper.HasPermission(EnumHelper.Roles.Viewer, "Assets", "Edit");

            // ASSERT
            Assert.IsFalse(hasPermission);
        }

        [TestMethod]
        public void HasPermission_AuthorAccessingEditNoPermissionDefined_True()
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
            bool hasPermission = permissionHelper.HasPermission(EnumHelper.Roles.Author, "Assets", "Edit");

            // ASSERT
            Assert.IsTrue(hasPermission);
        }

        [TestMethod]
        public void HasPermission_AuthorAccessingDeleteNoPermissionDefined_True()
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
            bool hasPermission = permissionHelper.HasPermission(EnumHelper.Roles.Author, "Assets", "Delete");

            // ASSERT
            Assert.IsTrue(hasPermission);
        }

        [TestMethod]
        public void HasPermission_AuthorAccessingDeletePermissionDenied_False()
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
                    Controller = "Assets",
                    Action = "Delete",
                    Role = "Author",
                    IsAllowed = false
                },
            };
            PermissionHelper permissionHelper = new PermissionHelper(rolePermissions.AsQueryable());

            // ACT
            bool hasPermission = permissionHelper.HasPermission(EnumHelper.Roles.Author, "Assets", "Delete");

            // ASSERT
            Assert.IsFalse(hasPermission);
        }


        [TestMethod]
        public void HasPermission_ViewerAccessingDeletePermissionExplicitlyAllowed_True()
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
                    Controller = "Assets",
                    Action = "Delete",
                    Role = "Viewer",
                    IsAllowed = true
                },
            };
            PermissionHelper permissionHelper = new PermissionHelper(rolePermissions.AsQueryable());

            // ACT
            bool hasPermission = permissionHelper.HasPermission(EnumHelper.Roles.Viewer, "Assets", "Delete");

            // ASSERT
            Assert.IsTrue(hasPermission);
        }


        [TestMethod]
        public void HasPermission_AuthorAccessingControllerPermissionExplicitlyDenied_False()
        {
            // ARRANGE
            List<RolePermission> rolePermissions = new List<RolePermission>
            {
                new RolePermission
                {
                    Controller = "LookupTypes",
                    Action = string.Empty,
                    Role = "Author",
                    IsAllowed = false
                },
                new RolePermission
                {
                    Controller = "Assets",
                    Action = "Delete",
                    Role = "Viewer",
                    IsAllowed = true
                },
            };
            PermissionHelper permissionHelper = new PermissionHelper(rolePermissions.AsQueryable());

            // ACT
            bool hasPermission = permissionHelper.HasPermission(EnumHelper.Roles.Author, "LookupTypes", "Delete");

            // ASSERT
            Assert.IsFalse(hasPermission);
        }
    }
}
