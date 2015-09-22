using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardwareInventoryManager.Helpers;
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

        [TestMethod]
        public void EncodeUrlCode()
        {
            // ARRANGE
            string code = "RKmpm1WWoOvUcvZrlAOLrTp1Kn93waPrizXTyJ3Tft/BSd1gNB+2vt9vVNCgBWJZaPOU2NYqoozUaWGzEjjeW+APrietgfhC3YwxhuECbijATJU+fCYvMVY/hynnia3k2R8sZh4O9+2O+Uf3eXCJyKZjSj1c6TY5aIoXidbJVD8rHnmCr0+oJlzMhlyaQjIz";
            
            // ACT
            string encoded = UtilityHelper.EncodeUrlCode(code);

            // ASSERT
            Assert.AreEqual("RKmpm1WWoOvUcvZrlAOLrTp1Kn93waPrizXTyJ3Tft%2fBSd1gNB%2b2vt9vVNCgBWJZaPOU2NYqoozUaWGzEjjeW%2bAPrietgfhC3YwxhuECbijATJU%2bfCYvMVY%2fhynnia3k2R8sZh4O9%2b2O%2bUf3eXCJyKZjSj1c6TY5aIoXidbJVD8rHnmCr0%2boJlzMhlyaQjIz", encoded);
        }

        [TestMethod]
        public void DecodeUrlCode()
        {
            // ARRANGE
            string code = "RKmpm1WWoOvUcvZrlAOLrTp1Kn93waPrizXTyJ3Tft%2fBSd1gNB%2b2vt9vVNCgBWJZaPOU2NYqoozUaWGzEjjeW%2bAPrietgfhC3YwxhuECbijATJU%2bfCYvMVY%2fhynnia3k2R8sZh4O9%2b2O%2bUf3eXCJyKZjSj1c6TY5aIoXidbJVD8rHnmCr0%2boJlzMhlyaQjIz";
            // ACT
            string decoded = UtilityHelper.DecodeUrlCode(code);

            // ASSERT
            Assert.AreEqual("RKmpm1WWoOvUcvZrlAOLrTp1Kn93waPrizXTyJ3Tft/BSd1gNB+2vt9vVNCgBWJZaPOU2NYqoozUaWGzEjjeW+APrietgfhC3YwxhuECbijATJU+fCYvMVY/hynnia3k2R8sZh4O9+2O+Uf3eXCJyKZjSj1c6TY5aIoXidbJVD8rHnmCr0+oJlzMhlyaQjIz", decoded);
        }
    }
}
