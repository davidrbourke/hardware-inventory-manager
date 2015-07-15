using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.ViewModels;
using System.Collections.Generic;

namespace HardwareInventoryManager.Tests.Mappings
{
    [TestClass]
    public class MappingTests
    {
        [TestMethod]
        public void Mapping_AssetToAssetViewModel_Maps()
        {
            // Arrange
            Mapper.CreateMap<Asset, AssetViewModel>();

            Asset a = new Asset
            {
                AssetId = 2,
                CategoryId = 2,
                LocationDescription = "Room",
                ObsolescenseDate = null
            };
            a.CreatedDate = new DateTime(2014, 7, 31);
            a.CategoryId = 1;
            var avm = Mapper.DynamicMap<Asset, AssetViewModel>(a);

            // Assert
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void Mapping_AssetListToAssetViewModelList_Maps()
        {
            // Arrange
            Mapper.CreateMap<IList<Asset>, IList<AssetViewModel>>();

            // Assert
            Mapper.AssertConfigurationIsValid();
        }
    }
}
