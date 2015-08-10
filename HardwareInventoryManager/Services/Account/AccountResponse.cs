using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Helpers.Account
{
    /// <summary>
    /// Class for returns login/account status messages from AccountService to client (e.g. controller)
    /// </summary>
    public class AccountResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}