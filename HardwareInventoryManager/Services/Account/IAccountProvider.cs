using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareInventoryManager.Services.Account
{
    /// <summary>
    /// Interface for Account Providers, implement this Interface to change the 3rd party 
    /// account provider (e.g. from ASP.NET Identity 2.0 to something else)
    /// </summary>
    public interface IAccountProvider
    {
        Task<ApplicationUser> Find(string userName, string password);
        Task<bool> IsLockedOut(string userId);
        Task<bool> ResetAccessCount(string userId);
        Task<ApplicationUser> Find(string username);
        Task<int> GetAccessFailedCount(string userId);
        Task<bool> LockUserAccount(string userId, DateTime? lockedOutTillDate);
        Task<bool> LoginFailed(string userId);
        Task<bool> SignIn(ApplicationUser user, bool isPermanent);
        bool Create(ApplicationUser applicationUser, string password);
        bool AddToRole(string userId, string role);
        Task<string> GeneratePasswordResetToken(string userId);
        Task<string> GenerateEmailConfirmationToken(string userId);

    }
}
