using HardwareInventoryManager.Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HardwareInventoryManager.Services.Account
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


        public async Task<AccountResponse> ForgotPassword(string userName, string url)
        {

            var userToReset = await _accountProvider.Find(userName);
            
            string code = await _accountProvider.GeneratePasswordResetToken(userToReset.Id);
            var callbackUrl =
                string.Format("{0}{1}/{2}?userId={3}&code={4}",
                url, "Account", "ResetPassword", userToReset.Id, code);
           
            IProcessEmail processEmail = new ProcessEmail(userName);
            processEmail.SendPasswordResetEmail(userToReset, callbackUrl);

            return new AccountResponse
            {
                Success = true,
            };

        }
        struct orr
        {
            public string userId { get; set; }
            public string  code { get; set; }
        }
        public async Task<AccountResponse> RequestEmailConfirmation(string userName, string url)
        {
            var recipientUser = await _accountProvider.Find(userName);
   
            string emailConfirmationToken = await _accountProvider.GenerateEmailConfirmationToken(recipientUser.Id);
            var callback =
                string.Format("{0}{1}/{2}?userId={3}&code={4}",
                url, "Account", "ConfirmEmail",
                recipientUser.Id,
                emailConfirmationToken);
           
            IProcessEmail processEmail = new ProcessEmail(userName);
            processEmail.SendEmailConfirmationEmail(recipientUser, callback);
            return new AccountResponse
            {
                Success = true,
            };
        }
    }
}