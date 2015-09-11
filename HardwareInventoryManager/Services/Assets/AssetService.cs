using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace HardwareInventoryManager.Services.Assets
{
    public class AssetService
    {
        public string UserName { get; set; }

        public AssetService(string userName)
        {
            UserName = userName;
        }


        public IQueryable<Asset> GetAllAssets()
        {
            var assets = Repository.GetAll()
                .Include(x => x.AssetMake)
                .Include(x => x.Category)
                .Include(x => x.WarrantyPeriod)
                .Include(x => x.NetworkedAssetDetail);
            return assets;   
        }

        public Asset GetSingleAsset(int id)
        {
            return GetAllAssets().FirstOrDefault(a => a.AssetId == id);
        }

        public void SaveAsset(Asset asset)
        {
            if(asset.AssetId == 0)
            {
                Repository.Create(asset);
                Repository.Save();
                return;
            }

            Repository.Edit(asset);
            Repository.Save();
        }

        public void Delete(int id)
        {
            Asset asset = Repository.Find(x => x.AssetId == id).First();
            Repository.Delete(asset);
            Repository.Save();
        }

        private IRepository<Asset> _repository;
        public IRepository<Asset> Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new AssetRepository(UserName);
                };
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }
    }
}