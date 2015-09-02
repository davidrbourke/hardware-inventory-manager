using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Services.Assets;
using System;
using System.Collections.Generic;
using System.IO;
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

        private IRepository<BulkImport> _bulkImportRepository;
        public IRepository<BulkImport> BulkImportRepository
        {
            get
            {
                if(_bulkImportRepository == null)
                {
                    _bulkImportRepository = new RepositoryWithoutTenant<BulkImport>();
                }
                return _bulkImportRepository;
            }
            set
            {
                _bulkImportRepository = value;
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

        public string BatchId { get; set; }

        public IEnumerable<Asset> PrepareImport(HttpPostedFileBase importedCsv)
        {
            string csvRaw = ConvertCsvFileToString(importedCsv);
            BatchId = BackupImport(csvRaw);
            string[] csvLines = ProcessCsvLines(csvRaw);
            string[] csvHeader = ProcessCsvHeader(csvLines[0]);
            IList<Asset> assetsToCommit = new List<Asset>();
            for (int i = 1; i < csvLines.Length; i++)
            {
                Asset asset = ProcessLineToAsset(csvHeader, csvLines[i]);
                assetsToCommit.Add(asset);
                asset.TenantId = 3; // TODO: REPLACE
                asset.AssetMakeId = 1;
                asset.CategoryId = 5;
                asset.WarrantyPeriodId = asset.WarrantyPeriod.LookupId;
            }
            return assetsToCommit;
        }

        public string ConvertCsvFileToString(HttpPostedFileBase importedCsv)
        {
            BinaryReader reader = new BinaryReader(importedCsv.InputStream);
            int csvLength = (int)importedCsv.InputStream.Length;
            byte[] byteInput = reader.ReadBytes(csvLength);

            string csvRaw = System.Text.Encoding.UTF8.GetString(byteInput);
            return csvRaw;
        }

        public string BackupImport(string importCsvAsString)
        {
            BulkImport bulkImport = new BulkImport
            {
                ImportText = importCsvAsString,
                TenantId = 3
            };
            BulkImportRepository.Create(bulkImport);
            BulkImportRepository.Save();
            return bulkImport.BulkImportId.ToString();
        }

        public void CommitImport(IEnumerable<Asset> assets)
        {
            foreach (Asset asset in assets)
            {
                AssetService assetService = new AssetService(_userName);
                assetService.SaveAsset(asset);
            }
        }

        public int ProcessCommit(string batchId)
        {
            int bulkImportId = int.Parse(batchId);
            BulkImport bulkImport = BulkImportRepository.Single(x => x.BulkImportId == bulkImportId);
            string[] csvLines = ProcessCsvLines(bulkImport.ImportText);
            string[] csvHeader = ProcessCsvHeader(csvLines[0]);
            IList<Asset> assetsToCommit = new List<Asset>();
            for (int i = 1; i < csvLines.Length; i++)
            {
                Asset asset = ProcessLineToAsset(csvHeader, csvLines[i]);
                assetsToCommit.Add(asset);
                asset.TenantId = 3; // TODO: REPLACE
                asset.AssetMakeId = 1;
                asset.CategoryId = 5;
                asset.WarrantyPeriodId = asset.WarrantyPeriod.LookupId;
            }
            CommitImport(assetsToCommit);
            return assetsToCommit.Count();
        }
    }
}