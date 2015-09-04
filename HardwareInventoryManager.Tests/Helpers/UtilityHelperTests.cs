using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardwareInventoryManager.Services;
using System.Text.RegularExpressions;

namespace HardwareInventoryManager.Tests.Helpers
{
    [TestClass]
    public class UtilityHelperTests
    {
        [TestMethod]
        public void GeneratePassword_ReturnsPasswordMeetingCriteria()
        {
            // ARRANGE
            UtilityHelper utilityHelper = new UtilityHelper();

            // ACT
            string generatedPwd = utilityHelper.GeneratePassword();

            // ASSERT
            Regex upperCaseRegex = new Regex("[A-Z]+");
            Regex lowerCaseRegex = new Regex("[a-z]+");
            Regex numberRegex = new Regex("[0-9]+");
            Regex specialCharacterRegex = new Regex("[!\"#$%&'()*+-./]+");

            Assert.IsTrue(upperCaseRegex.IsMatch(generatedPwd));
            Assert.IsTrue(lowerCaseRegex.IsMatch(generatedPwd));
            Assert.IsTrue(numberRegex.IsMatch(generatedPwd));
            Assert.IsTrue(specialCharacterRegex.IsMatch(generatedPwd));
            Assert.AreEqual(generatedPwd.Length, 8);
        }
    }
}
