using HardwareInventoryManager.Controllers;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HardwareInventoryManager.Tests.Controllers
{
    [TestClass]
    public class AssetsControllerTests
    {
        [TestMethod, TestCategory("Controller")]
        public void Index_NoAssets_NoDataReturned()
        {
            // ARRANGE
            var mock = new Mock<IRepository<Asset>>();
            mock.Setup(r => r.GetAll()).Returns(new List<Asset>().AsQueryable());

            AssetsController controller = new AssetsController(mock.Object);

            // ACT
            ViewResult result = controller.Index() as ViewResult;
            var model = result.Model as IEnumerable<AssetViewModel>;

            // ASSERT 
            Assert.AreEqual(0, model.Count());
        }

        [TestMethod, TestCategory("Controller")]
        public void Index_CountAssets_HasAssets()
        {
            // ARRANGE
            var mock = new Mock<IRepository<Asset>>();
            mock.Setup(r => r.GetAll()).Returns(MultipleAssets());

            AssetsController controller = new AssetsController(mock.Object);

            // ACT
            ViewResult result = controller.Index() as ViewResult;
            var model = result.Model as IEnumerable<AssetViewModel>;

            // ASSERT 
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod, TestCategory("Controller")]
        public void Detail_ValidId_Loaded()
        {
            // ARRANGE
            var mock = new Mock<Repository<Asset>>();
     
            mock.Setup(r => r.Find(It.IsAny<Expression<Func<Asset, bool>>>())).Returns(
                MultipleAssets());

            AssetsController controller = new AssetsController(mock.Object);

            // ACT
            ViewResult result = controller.Details(1) as ViewResult;

            // ASSERT
            var model = result.Model as AssetViewModel;
            Assert.AreEqual(MultipleAssets().First().AssetId, model.AssetId);
            Assert.AreEqual(MultipleAssets().First().Model, model.Model);
        }

        [TestMethod, TestCategory("Controller")]
        public void Create_Valid_RedirectsToIndex()
        {
            // ARRANGE
            var mock = new Mock<Repository<Asset>>();

            mock.Setup(r => r.Create(It.IsAny<Asset>())).
                Returns(new Asset
                {
                    AssetId = 1,
                    Model = "123"
                });

            AssetViewModel avm = new AssetViewModel
            {
                AssetId = 1,
                Model = "123"
            };

            AssetsController controller = new AssetsController(mock.Object);

            // ACT
            RedirectToRouteResult result = controller.Create(avm) as RedirectToRouteResult;

            // ASSERT
            Assert.AreEqual("action", result.RouteValues.Keys.First());
            Assert.AreEqual("Index", result.RouteValues.Values.First());
        }

        [TestMethod, TestCategory("Controller")]
        public void Create_InValidModelState_ReturnsErrors()
        {
            // ARRANGE
            var mock = new Mock<Repository<Asset>>();

            AssetViewModel avm = new AssetViewModel();
            
            AssetsController controller = new AssetsController(mock.Object);
            controller.ModelState.Clear();
            controller.ModelState.AddModelError("Model is a required field", "Test Exception");
            // ACT
            ViewResult result = controller.Create(avm) as ViewResult;

            // ASSERT
            Assert.AreEqual(EnumHelper.Alerts.Error.ToString(), result.TempData.Keys.First());         
        }

        #region Private Helper Methods
        private IQueryable<Asset> MultipleAssets()
        {
            var collection = new List<Asset>
            {
                new Asset
                {
                    AssetId = 1,
                    Model = "123"
                },
                new Asset
                {
                    AssetId = 2,
                    Model = "456"
                }
            };
            return collection.AsQueryable();
        }


        #endregion
    }
}
