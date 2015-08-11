using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services.Messaging
{
    public interface IEmailService
    {
        void SendEmail(string recipient, string subject, string body);
        int TenantId { get; set; }
    }
}