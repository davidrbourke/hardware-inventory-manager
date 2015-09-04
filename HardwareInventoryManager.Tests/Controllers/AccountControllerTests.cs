using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardwareInventoryManager.Controllers;
using Moq;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using HardwareInventoryManager.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Security.Claims;
using HardwareInventoryManager.Services.Account;


namespace HardwareInventoryManager.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void  Login_FakeUser_LoginFailsWithGenericMessage()
        {
            // ARRANGE
            Mock<IAccountProvider> accountProvider = new Mock<IAccountProvider>();
            ApplicationUser user = null;
            accountProvider.Setup(x => x.Find("something@something.com", string.Empty)).Returns(Task.FromResult(user));
            AccountService accountService = new AccountService(accountProvider.Object);
            
            // ACT
            Task<AccountResponse> response = accountService.Login("something@something.com", "anything");
            response.Wait();
            
            // ASSERT
            Assert.IsFalse(response.Result.Success);
            Assert.AreEqual(HIResources.Strings.Login_Error, response.Result.Message);
        }

        [TestMethod]
        public void Login_IncorrectPassword_LoginFailsWithGenericMessage()
        {
            // ARRANGE
            Mock<IAccountProvider> accountProvider = new Mock<IAccountProvider>();
            ApplicationUser user = null;
            ApplicationUser failedUser = new ApplicationUser
            {
                Email = "something@something.com"
            };
            accountProvider.Setup(x => x.Find("something@something.com", string.Empty)).Returns(Task.FromResult(user));
            accountProvider.Setup(x => x.Find("something@something.com")).Returns(Task.FromResult(failedUser));
            AccountService accountService = new AccountService(accountProvider.Object);

            // ACT
            Task<AccountResponse> response = accountService.Login("something@something.com", "anything");
            response.Wait();

            // ASSERT
            Assert.IsFalse(response.Result.Success);
            Assert.AreEqual(HIResources.Strings.Login_Error, response.Result.Message);
            accountProvider.Verify(x => x.LoginFailed(It.IsAny<string>()), Times.AtLeastOnce);            
        }

        [TestMethod]
        public void Login_IsLockedOut_LoginFailsWithGenericMessage()
        {
            // ARRANGE
            Mock<IAccountProvider> accountProvider = new Mock<IAccountProvider>();
            ApplicationUser user = new ApplicationUser
            {
                Email = "something@something.com",
            };
            
            accountProvider.Setup(x => x.Find("something@something.com", It.IsAny<string>())).Returns(Task.FromResult(user));
            accountProvider.Setup(x => x.IsLockedOut(It.IsAny<string>())).Returns(Task.FromResult(true));
            AccountService accountService = new AccountService(accountProvider.Object);

            // ACT
            Task<AccountResponse> response = accountService.Login("something@something.com", "anything");
            response.Wait();

            // ASSERT
            Assert.IsFalse(response.Result.Success);
            Assert.AreEqual(HIResources.Strings.Login_Error, response.Result.Message);
        }

        [TestMethod]
        public void Login_ValidUser_LoginSucceeds()
        {
            // ARRANGE
            Mock<IAccountProvider> accountProvider = new Mock<IAccountProvider>();
            ApplicationUser user = new ApplicationUser
            {
                Email = "something@something.com",
            };

            accountProvider.Setup(x => x.Find("something@something.com", It.IsAny<string>())).Returns(Task.FromResult(user));
            accountProvider.Setup(x => x.IsLockedOut(It.IsAny<string>())).Returns(Task.FromResult(false));
            accountProvider.Setup(x => x.SignIn(It.IsAny<ApplicationUser>(), false)).Returns(Task.FromResult(true));
            AccountService accountService = new AccountService(accountProvider.Object);

            // ACT
            Task<AccountResponse> response = accountService.Login("something@something.com", "anything");
            response.Wait();

            // ASSERT
            Assert.IsTrue(response.Result.Success);
            Assert.AreEqual(string.Empty, response.Result.Message);
        }

        [TestMethod]
        public void Login_ThirdFailedLogin_AccountLocked()
        {
            // ARRANGE
            Mock<IAccountProvider> accountProvider = new Mock<IAccountProvider>();
            ApplicationUser user = null;
            ApplicationUser failedUser = new ApplicationUser
            {
                Email = "something@something.com"
            };
            accountProvider.Setup(x => x.Find("something@something.com", string.Empty)).Returns(Task.FromResult(user));
            accountProvider.Setup(x => x.Find("something@something.com")).Returns(Task.FromResult(failedUser));
            accountProvider.Setup(x => x.GetAccessFailedCount(It.IsAny<string>())).Returns(Task.FromResult(2));
            AccountService accountService = new AccountService(accountProvider.Object);

            // ACT
            Task<AccountResponse> response = accountService.Login("something@something.com", "anything");
            response.Wait();

            // ASSERT
            Assert.IsFalse(response.Result.Success);
            Assert.AreEqual(HIResources.Strings.Login_Error, response.Result.Message);
            accountProvider.Verify(x => x.LockUserAccount(It.IsAny<string>(), It.IsAny<DateTime>()), Times.AtLeastOnce);   
        }
    }
}