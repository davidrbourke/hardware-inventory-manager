using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services
{
    public static class EnumHelper
    {
        public enum LookupTypes
        {
            Make,
            Category,
            WarrantyPeriod
        }

        public enum Alerts
        {
            Success,
            Warning,
            Info,
            Error
        }

        public enum Roles
        {
            Admin,
            Author,
            Viewer
        }
    }
}