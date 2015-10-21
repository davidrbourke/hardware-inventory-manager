using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace HardwareInventoryManager.Services.Reporting
{
    public class ReportingService
    {
        private string _username;

        public ReportingService(string username)
        {
            this._username = username;
        }
        /// <summary>
        /// Returns list of assets where the warranty has expired
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Asset> ExpiredWarrantyReport()
        {
            IRepository<Asset> rep = new Repository<Asset>(_username);
            IList<Asset> assets = rep.GetAll().Include(x => x.WarrantyPeriod).Include(x =>x.AssetMake).ToList();
            return assets.Where(x => x.WarrantyExpiryDate < DateTime.Now);
        }

        /// <summary>
        /// Returns a list of assets where the obsolesence date is in the past
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Asset> PastObsoleteDateReport()
        {
            IRepository<Asset> rep = new Repository<Asset>(_username);
            IList<Asset> assets = rep.GetAll().Include(x => x.AssetMake).ToList();
            return assets.Where(x => x.ObsolescenseDate < DateTime.Now);
        }
    }
}