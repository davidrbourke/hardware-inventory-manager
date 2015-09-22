using HardwareInventoryManager.Models;
using HardwareInventoryManager.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareInventoryManager.Controllers
{
    public interface IAppController
    {
        void Alert(EnumHelper.Alerts alertType, string message);
        ApplicationUser GetCurrentUser();
        int GetTenantContextId();
    }
}
