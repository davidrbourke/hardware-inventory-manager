using HardwareInventoryManager.Services;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Services.Assets;
using HardwareInventoryManager.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using AutoMapper;
using System.Text;

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

        private IRepository<LookupType> _lookupTypesRepository;
        public IRepository<LookupType> LookupTypesRepository
        {
            get
            {
                if(_lookupTypesRepository ==null)
                {
                    _lookupTypesRepository = new RepositoryWithoutTenant<LookupType>();
                }
                return _lookupTypesRepository;
            }
            set
            {
                _lookupTypesRepository = value;
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
        public ConvertedAsset ProcessLineToAsset(string[] header, string line, int tenantId, int rowId)
        {
            ConvertedAsset convertedAsset = new ConvertedAsset();
            convertedAsset.Errors = new List<string>();
            string[] linesArray = line.Split(',');
            Asset asset = new Asset();
            for (int i = 0; i < linesArray.Length; i++)
            {
                asset.AssetId = rowId;
                switch (header[i].ToLower())
                {
                    case "model":
                        asset.Model = linesArray[i];
                        break;
                    case "serialnumber":
                        asset.SerialNumber = linesArray[i];
                        break;
                    case "purchasedate":
                        DateTime purchaseDate;
                        if (DateTime.TryParse(linesArray[i], out purchaseDate))
                        {
                            asset.PurchaseDate = purchaseDate;
                        }
                        else
                        {
                            convertedAsset.Errors.Add(string.Format(HIResources.Strings.ImportError_PurchaseDate, rowId, linesArray[i]));
                        }
                        break;
                    case "warrantyperiod":
                        string warrantyPeriodDescription = linesArray[i];
                        Lookup warrantyPeriod = 
                        LookupRepository.Single(
                            x => x.Type.Description == EnumHelper.LookupTypes.WarrantyPeriod.ToString()
                                && x.Description.Equals(warrantyPeriodDescription, 
                                StringComparison.InvariantCultureIgnoreCase));
                        if (warrantyPeriod == null)
                        {
                            LookupType type = LookupTypesRepository.Single(
                                x => x.Description == EnumHelper.LookupTypes.WarrantyPeriod.ToString());
                            warrantyPeriod = new Lookup
                            {
                                Description = warrantyPeriodDescription,
                                LookupTypeId = type.LookupTypeId,
                                TenantId = tenantId
                            };
                        }
                        asset.WarrantyPeriod = warrantyPeriod;
                        break;
                    case "obsolescencedate":
                        DateTime obsolescenseDate;
                        if (DateTime.TryParse(linesArray[i], out obsolescenseDate))
                        {
                            asset.ObsolescenseDate = obsolescenseDate;
                        }
                        else
                        {
                            convertedAsset.Errors.Add(string.Format(HIResources.Strings.ImportError_ObsolescenseDate, rowId, linesArray[i]));
                        }
                        break;
                    case "pricepaid":
                        decimal pricePaid;
                        if(decimal.TryParse(linesArray[i], out pricePaid))
                        {
                            asset.PricePaid = pricePaid;
                        }
                        else
                        {
                            convertedAsset.Errors.Add(string.Format(HIResources.Strings.ImportError_PricePaid, rowId, linesArray[i]));
                        }
                        break;
                    case "assetmake":
                        string assetMakeDescription = linesArray[i];
                        Lookup assetMake =
                            LookupRepository.Single(
                            x=> x.Type.Description == EnumHelper.LookupTypes.Make.ToString()
                                && x.Description.Equals(assetMakeDescription,
                                StringComparison.InvariantCultureIgnoreCase));
                        if (assetMake == null)
                        {
                            LookupType type = LookupTypesRepository.Single(
                                x => x.Description == EnumHelper.LookupTypes.Make.ToString());
                            assetMake = new Lookup
                            {
                                Description = assetMakeDescription,
                                LookupTypeId = type.LookupTypeId,
                                TenantId = tenantId
                            };
                        }
                        asset.AssetMake = assetMake;
                        break;
                    case "category":
                        string categoryDescription = linesArray[i];

                        Lookup category =
                            LookupRepository.Single(
                            x => x.Type.Description == EnumHelper.LookupTypes.Category.ToString()
                                && x.Description.Equals(categoryDescription,
                                StringComparison.InvariantCultureIgnoreCase));
                        if (category == null)
                        {
                            LookupType type = LookupTypesRepository.Single(
                                x => x.Description == EnumHelper.LookupTypes.Category.ToString());
                            category = new Lookup
                            {
                                Description = categoryDescription,
                                LookupTypeId = type.LookupTypeId,
                                TenantId = tenantId
                            };
                        }
                        asset.Category = category;
                        break;
                    case "locationdescription":
                        asset.LocationDescription = linesArray[i];
                        break;
                    default:
                        break;
                }
            }
            if(string.IsNullOrWhiteSpace(asset.Model))
            {
                convertedAsset.Errors.Add(string.Format(HIResources.Strings.ImportError_ModelMissing, asset.AssetId));
            }  
            if(string.IsNullOrWhiteSpace(asset.Model))
            {
                convertedAsset.Errors.Add(string.Format(HIResources.Strings.ImportError_SerialNumberMissing, asset.AssetId));
            }  
            convertedAsset.Asset = asset;
            return convertedAsset;
        }

        public string BatchId { get; set; }


        public BulkUploadViewModel PrepareImport(int batchId)
        {
            BulkImport bulkImport = BulkImportRepository.Single(x => x.BulkImportId == batchId);
            if (bulkImport != null)
            {
                BulkUploadViewModel assetsToCommit = BuildAssetsForDisplay(bulkImport.ImportText, 0);
                return assetsToCommit;
            }
            return new BulkUploadViewModel();
        }

        public BulkUploadViewModel PrepareImport(HttpPostedFileBase importedCsv)
        {
            BulkImport bulkImportResponse = new BulkImport();
            string csvRaw = ConvertCsvFileToString(importedCsv);
            BatchId = BackupImport(csvRaw);
            BulkUploadViewModel assetsToCommit = BuildAssetsForDisplay(csvRaw, 0);
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

        public int ProcessCommit(string batchId, TenantViewModel tenant)
        {
            int bulkImportId = int.Parse(batchId);
            BulkImport bulkImport = BulkImportRepository.Single(x => x.BulkImportId == bulkImportId);

            ConvertedAssetsDto convertedAssets = BuildAssetsClearLookups(bulkImport.ImportText, tenant.TenantId);

            CommitImport(convertedAssets.Assets);
            return convertedAssets.Assets.Count();
        }


        private ConvertedAssetsDto BuildAssetsClearLookups(string rawCsv, int tenantId)
        {
            
            ConvertedAssetsDto bulkUpload = BuildAssets(rawCsv, tenantId);
            foreach(Asset asset in bulkUpload.Assets)
            {
                ClearLookups(asset);
            }
            return bulkUpload;
        }

        private BulkUploadViewModel BuildAssetsForDisplay(string rawCsv, int tenantId)
        {
            string[] csvRawLines = ProcessCsvLines(rawCsv);
            string[] csvHeader = ProcessCsvHeader(csvRawLines[0]);

            IList<Asset> assets = new List<Asset>();
            IList<List<string>> errors = new List<List<string>>();

            string[] csvLines = RemoveBlankLines(csvRawLines);

            for (int i = 1; i < csvLines.Length; i++)
            {
                ConvertedAsset convertedAsset = ProcessLineToAsset(csvHeader, csvLines[i], tenantId, i);
                assets.Add(convertedAsset.Asset);
                if (convertedAsset.Errors.Count > 0)
                {
                    errors.Add(convertedAsset.Errors);
                }
                convertedAsset.Asset.TenantId = tenantId;
                convertedAsset.Asset.AssetMakeId = convertedAsset.Asset.AssetMake.LookupId;
                convertedAsset.Asset.CategoryId = convertedAsset.Asset.Category.LookupId;
                convertedAsset.Asset.WarrantyPeriodId = convertedAsset.Asset.WarrantyPeriod.LookupId;
            }
            Mapper.CreateMap<Asset, AssetViewModel>();
            var assetsForView = Mapper.Map<IList<AssetViewModel>>(assets);

            BulkUploadViewModel response = new BulkUploadViewModel
            {
                Errors = errors,
                Assets = assetsForView
            };
            return response;
        }

        public string[] RemoveBlankLines(string[] csvLines)
        {
            StringBuilder buildingNewLines = new StringBuilder();
            foreach(string line in csvLines)
            {
                string[] columns = line.Split(',');
                int countOfNonBlanks = columns.Count(x => !string.IsNullOrWhiteSpace(x));
                if(countOfNonBlanks > 0)
                {
                    buildingNewLines.AppendLine(line);
                }
            }
            char[] chars = {'\r','\n'};
            string[] lines = buildingNewLines.ToString().Split(chars, StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }

        private ConvertedAssetsDto BuildAssets(string rawCsv, int tenantId)
        {
            string[] csvRawLines = ProcessCsvLines(rawCsv);
            string[] csvHeader = ProcessCsvHeader(csvRawLines[0]);

            string[] csvLines = RemoveBlankLines(csvRawLines);

            IList<Asset> assets = new List<Asset>();
            IList<List<string>> errors = new List<List<string>>();
            for (int i = 1; i < csvLines.Length; i++)
            {
                ConvertedAsset convertedAsset = ProcessLineToAsset(csvHeader, csvLines[i], tenantId, i);
                if (!string.IsNullOrWhiteSpace(convertedAsset.Asset.Model)
                    && !string.IsNullOrWhiteSpace(convertedAsset.Asset.SerialNumber))
                {

                    assets.Add(convertedAsset.Asset);
                    errors.Add(convertedAsset.Errors);
                    convertedAsset.Asset.TenantId = tenantId;
                    convertedAsset.Asset.AssetMakeId = convertedAsset.Asset.AssetMake.LookupId;
                    convertedAsset.Asset.CategoryId = convertedAsset.Asset.Category.LookupId;
                    convertedAsset.Asset.WarrantyPeriodId = convertedAsset.Asset.WarrantyPeriod.LookupId;
                }
            }
            
            ConvertedAssetsDto response = new ConvertedAssetsDto
            {
                Errors = errors.ToList(),
                Assets = assets.ToList()
            };
            return response;
        }

        private void ClearLookups(Asset asset)
        {
            asset.AssetId = 0;
            asset.AssetMake = asset.AssetMakeId != 0 ? null : asset.AssetMake;
            asset.Category = asset.CategoryId != 0 ? null : asset.Category;
            asset.WarrantyPeriod = asset.WarrantyPeriodId != 0 ? null : asset.WarrantyPeriod;
        }
    }

    public struct ConvertedAsset
    {
        public List<string> Errors { get; set; }
        public Asset Asset { get; set; }
    }


    public struct ConvertedAssetsDto
    {
        public List<List<string>> Errors { get; set; }
        public List<Asset> Assets { get; set; }
    }
}