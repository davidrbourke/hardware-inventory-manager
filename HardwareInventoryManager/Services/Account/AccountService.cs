using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HardwareInventoryManager.Helpers.Account
{
    /// <summary>
    /// Account Service class for account functionality
    /// </summary>
    public class AccountService
    {
        private IAccountProvider _accountProvider;
        private const int MaxFailedLoginAttempts = 3;

        public AccountService(IAccountProvider accountProvider)
        {
            _accountProvider = accountProvider;
        }

        /// <summary>
        /// Attempt login to the application
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AccountResponse> Login(string userName, string password)
        {
            var user = await _accountProvider.Find(userName, password);
            if (user != null)
            {
                var isLockedOut = await _accountProvider.IsLockedOut(user.Id);
                if (isLockedOut)
                {
                    return new AccountResponse
                    {
                        Success = false,
                        Message = HIResources.Strings.Login_Error
                    };
                }
                else
                {
                    bool result = await _accountProvider.ResetAccessCount(user.Id);
                    var loggedIn = await _accountProvider.SignIn(user, false);

                    return new AccountResponse
                    {
                        Success = true,
                        Message = string.Empty
                    };
                }
            }
            else
            {
                var failedUser = await _accountProvider.Find(userName);
                if (failedUser != null)
                {
                    int failedCount = await _accountProvider.GetAccessFailedCount(failedUser.Id);
                    if (failedCount == MaxFailedLoginAttempts - 1)
                    {
                        bool result = await _accountProvider.LockUserAccount(failedUser.Id, DateTime.MaxValue);
                    }
                    else
                    {
                        var result = await _accountProvider.LoginFailed(failedUser.Id);
                    }
                }
                return new AccountResponse
                {
                    Success = false,
                    Message = HIResources.Strings.Login_Error
                };
            }
        }
    }
}