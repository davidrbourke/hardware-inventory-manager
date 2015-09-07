using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HardwareInventoryManager.Controllers;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Services.Import;
using Moq;
using HardwareInventoryManager.Repository;
using System.Linq.Expressions;


namespace HardwareInventoryManager.Tests.Services
{
    [TestClass]
    public class ImportServiceTests
    {
        [TestMethod]
        public void Upload_CSVWithData_Processed()
        {
            // ARRANGE
            var service = new ImportService("david");

            string csvString = 
@"Model,Serial Number,Purchase Date,Warranty Period,Obsolescense Date,Price Paid,Category,Location Description
12345,LLLLLLLL1,10/03/2015,3 years,10/03/2018,100,Desktop,Room 101
456798,MMMMM1,10/03/2014,1 years,10/03/2017,200,Laptop,Room 102
";

            // ACT
            string[] lines = service.ProcessCsvLines(csvString);

            // ASSERT
            Assert.AreEqual(3, lines.Length);

        }

        [TestMethod]
        public void ProcessCsvHeader_CSVHeaderSupplied_Processed()
        {
            // ARRANGE
            var service = new ImportService("david");
            string header = "Model,Serial Number,Purchase Date,Warranty Period,Obsolescense Date,Price Paid,Category,Location Description";
            
            // ACT
            string[] headerArray = service.ProcessCsvHeader(header);

            // ASSERT
            Assert.AreEqual(8, headerArray.Length);
            Assert.AreEqual("Model", headerArray[0]);
            Assert.AreEqual("SerialNumber", headerArray[1]);
        }


        [TestMethod]
        public void ProcessLineToAsset_HeaderAndLineSupplied_Processes()
        {
            // ARRANGE
            Mock<IRepository<Lookup>> m = new Mock<IRepository<Lookup>>();
            m.Setup(x => x.Single(It.IsAny<Expression<Func<Lookup, bool>>>())).Returns(
                new Lookup
                {
                    Description = "3 Years",
                    Type = new LookupType
                    {
                        Description = "WarrantyPeriod"
                    }
                }
            );
            
            var service = new ImportService("david");
            string[] header = {
                                  "Model",
                                  "SerialNumber",
                                  "PurchaseDate",
                                  "WarrantyPeriod",
                                  "ObsolescenseDate",
                                  "PricePaid",
                                  "Category",
                                  "LocationDescription"
                              };
            string line = "12345,LLLLLLLL1,10/03/2015,3 years,10/03/2018,100,Desktop,Room 101";
            service.LookupRepository = m.Object;

            // ACT
            ConvertedAsset convertedAsset = service.ProcessLineToAsset(header, line, 3, 1);
            Asset asset = convertedAsset.Asset;

            // ASSERT
            Assert.IsNotNull(asset);
            Assert.AreEqual("12345", asset.Model);
            Assert.AreEqual("LLLLLLLL1", asset.SerialNumber);
            Assert.AreEqual("10/03/2015", asset.PurchaseDate.Value.ToString("dd/MM/yyyy"));
            Assert.AreEqual("3 Years", asset.WarrantyPeriod.Description);

        }

        [TestMethod]
        public void ProcessLineToAsset_InvalidPurchaseDate_ErrorSupplied()
        {
            // ARRANGE
            Mock<IRepository<Lookup>> m = new Mock<IRepository<Lookup>>();
            m.Setup(x => x.Single(It.IsAny<Expression<Func<Lookup, bool>>>())).Returns(
                new Lookup
                {
                    Description = "3 Years",
                    Type = new LookupType
                    {
                        Description = "WarrantyPeriod"
                    }
                }
            );

            var service = new ImportService("david");
            string[] header = {
                                  "Model",
                                  "SerialNumber",
                                  "PurchaseDate",
                                  "WarrantyPeriod",
                                  "ObsolescenceDate",
                                  "PricePaid",
                                  "Category",
                                  "LocationDescription"
                              };
            string line = "12345,LLLLLLLL1,30/30/2015,3 years,july july aug,money,Desktop,Room 101";
            service.LookupRepository = m.Object;

            // ACT
            ConvertedAsset convertedAsset = service.ProcessLineToAsset(header, line, 3, 1);
            Asset asset = convertedAsset.Asset;

            // ASSERT
            Assert.IsNotNull(asset);
            Assert.AreEqual("12345", asset.Model);
            Assert.AreEqual("LLLLLLLL1", asset.SerialNumber);
            Assert.AreEqual("3 Years", asset.WarrantyPeriod.Description);
            Assert.AreEqual(3, convertedAsset.Errors.Count);
            Assert.AreEqual(
                string.Format(HIResources.Strings.ImportError_PurchaseDate, 1, "30/30/2015"),
                convertedAsset.Errors[0]);
            Assert.AreEqual(
                string.Format(HIResources.Strings.ImportError_ObsolescenseDate, 1, "july july aug"),
                convertedAsset.Errors[1]);
            Assert.AreEqual(string.Format(HIResources.Strings.ImportError_PricePaid, 1, "money"),
                convertedAsset.Errors[2]);
       
        }

        [TestMethod]
        public void RemoveBlankLines_BlankLines_Cleared()
        {
            // ARRANGE
            string[] linesWithBlanks = {
                ",,,,,,,",
                "12345,LLLLLLLL1,30/30/2015,3 years,july july aug,money,Desktop,Room 101",
                ",,,,,,,",
                "",
                "654321,LLLLLLLL1,30/30/2015,3 years,july july aug,money,Desktop,Room 101",
                ",,,,,,," };
            var service = new ImportService("user");

            // ACT
            string[] trimmedLines = service.RemoveBlankLines(linesWithBlanks);

            // ASSERT
            Assert.AreEqual(2, trimmedLines.Length);
            Assert.IsTrue(trimmedLines[0].StartsWith("12345"));
            Assert.IsTrue(trimmedLines[1].StartsWith("654321"));

        }
    }
}
