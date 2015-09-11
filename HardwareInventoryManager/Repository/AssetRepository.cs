using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Repository
{
    public class AssetRepository : Repository<Asset>
    {
        public AssetRepository(string userName)
            : base(new CustomApplicationDbContext(), userName)
        {
        }

        public AssetRepository(CustomApplicationDbContext context, string userName) : base(context, userName)
        {
        }


        public override Asset Edit(Asset asset)
        {
            IList<int> tenants = base.GetTenantIds();
            if (tenants.Contains(asset.TenantId))
            {
                asset.TenantId = asset.TenantId;
                dbContext.Entry(asset).State = System.Data.Entity.EntityState.Modified;
                if(asset.AssetDetailId == 0
                    && asset.NetworkedAssetDetail != null)
                {
                    asset.NetworkedAssetDetail.TenantId = asset.TenantId;
                    dbContext.Entry(asset.NetworkedAssetDetail).State = EntityState.Added;
                } else if(asset.AssetDetailId > 0
                    && asset.NetworkedAssetDetail != null)
                {
                    asset.NetworkedAssetDetail.TenantId = asset.TenantId;
                    dbContext.Entry(asset.NetworkedAssetDetail).State = EntityState.Modified;
                }
            }
            return asset;
        }
    }
}