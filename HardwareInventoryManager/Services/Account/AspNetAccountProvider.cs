using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using HardwareInventoryManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Microsoft.Owin;
using System.Data.Entity.Validation;

namespace HardwareInventoryManager.Helpers.Account
{
    /// <summary>
    /// Implementation of IAccountProvider using ASP.NET Identity 2.0
    /// </summary>
    public class AspNetAccountProvider : IAccountProvider
    {
        private ApplicationUserManager _userManager;
        private IAuthenticationManager _authenticationManager;

        public AspNetAccountProvider(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        public async Task<ApplicationUser> Find(string userName, string password)
        {
            return await _userManager.FindAsync(userName, password);
        }

        public async Task<bool> IsLockedOut(string userId)
        {
            return await _userManager.IsLockedOutAsync(userId);
        }

        public async Task<bool> ResetAccessCount(string userId)
        {
            IdentityResult result = await _userManager.ResetAccessFailedCountAsync(userId);
            return result.Succeeded;
        }

        public async Task<ApplicationUser> Find(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<int> GetAccessFailedCount(string userId)
        {
            return  await _userManager.GetAccessFailedCountAsync(userId);
        }

        public async Task<bool> LockUserAccount(string userId, DateTime? lockedOutTillDate)
        {
            IdentityResult result = await _userManager.SetLockoutEnabledAsync(userId, true);
            IdentityResult dateResult = await _userManager.SetLockoutEndDateAsync(userId, DateTime.MaxValue);
            return result.Succeeded && dateResult.Succeeded;
        }

        public async Task<bool> LoginFailed(string userId)
        {
            int failedLoginCount = _userManager.MaxFailedAccessAttemptsBeforeLockout = 3;
            IdentityResult accessFailed = await _userManager.AccessFailedAsync(userId);
            return accessFailed.Succeeded;
        }

        public async Task<bool> SignIn(ApplicationUser user, bool isPersistent)
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            _authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(_userManager));
            return true;
        }

        public bool Create(ApplicationUser applicationUser, string password)
        {
            IdentityResult result = _userManager.Create(applicationUser, password);
            return result.Succeeded;
        }

        public bool AddToRole(string userId, string role)
        {
            IdentityResult result = _userManager.AddToRole(userId, role);
            return result.Succeeded;
        }
    }
}