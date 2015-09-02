using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Services.Import
{
    public class ImportService
    {
        private string _userName;

        public ImportService(string userName)
        {
            _userName = userName;
        }

        private IRepository<Lookup> _lookupRepository;
        public IRepository<Lookup> LookupRepository
        {
            get
            {
                if (_lookupRepository == null)
                {
                    _lookupRepository = new RepositoryWithoutTenant<Lookup>();
                }
                return _lookupRepository;
            }
            set
            {
                _lookupRepository = value;
            }
        }

        /// <summary>
        /// Split the input by lines, including header and all lines
        /// First index in the returned array should be the header
        /// </summary>
        /// <param name="csv"></param>
        /// <returns></returns>
        public string[] ProcessCsvLines(string csv)
        {
            string[] charsToSplit = { "\r", "\n" };
            return csv.Split(charsToSplit, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Separate the header line into it's columns
        /// returns are array of the header columns
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public string[] ProcessCsvHeader(string line)
        {
            string[] header = line.Split(',');
            for (int i = 0; i < header.Length; i++)
            {
                string column = System.Text.RegularExpressions.Regex.Replace(header[i], @"\s+", "");
                header[i] = column;
            }
            return header;
        }

        /// <summary>
        /// Create and asset object that can be saved to the database for a single row in the csv file
        /// </summary>
        /// <param name="header"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public Asset ProcessLineToAsset(string[] header, string line)
        {
            string[] linesArray = line.Split(',');
            Asset asset = new Asset();
            for (int i = 0; i < linesArray.Length; i++)
            {
                switch (header[i])
                {
                    case "Model":
                        asset.Model = linesArray[i];
                        break;
                    case "SerialNumber":
                        asset.SerialNumber = linesArray[i];
                        break;
                    case "PurchaseDate":
                        asset.PurchaseDate = DateTime.Parse(linesArray[i]);
                        break;
                    case "WarrantyPeriod":

                        string warrantyPeriodDescription = linesArray[i];

                        Lookup warrantyPeriod = 
                        LookupRepository.Single(
                            x => x.Type.Description == EnumHelper.LookupTypes.WarrantyPeriod.ToString()
                                && x.Description.Equals(warrantyPeriodDescription, 
                                StringComparison.InvariantCultureIgnoreCase));
                        asset.WarrantyPeriod = warrantyPeriod;
                        break;
                    default:
                        break;
                }
            }

            return asset;
        }

        public void CommitAssets(IList<Asset> assets)
        {

            foreach(Asset asset in assets)
            {
                
            }
        }
    }
}