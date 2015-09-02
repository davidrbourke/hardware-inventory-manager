using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Services.Assets;
using HardwareInventoryManager.Services.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardwareInventoryManager.Controllers
{
    public class BulkUploadsController : Controller
    {
        // GET: BulkUploads
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase FileUpload)
        {

            BinaryReader reader = new BinaryReader(FileUpload.InputStream);


            int csvLength = (int)FileUpload.InputStream.Length;
            byte[] byteInput = reader.ReadBytes(csvLength);

            string csvRaw = System.Text.Encoding.UTF8.GetString(byteInput);

            ImportService importService = new ImportService(User.Identity.Name);
            string[] csvLines = importService.ProcessCsvLines(csvRaw);
            string[] csvHeader = importService.ProcessCsvHeader(csvLines[0]);
            IList<Asset> assetsToCommit = new List<Asset>();
            for (int i = 1; i < csvLines.Length; i++)
            {
                Asset asset = importService.ProcessLineToAsset(csvHeader, csvLines[i]);
                assetsToCommit.Add(asset);
                asset.TenantId = 3; // TODO: REPLACE
                asset.AssetMakeId = 1;
                asset.CategoryId = 5;
                asset.WarrantyPeriodId = asset.WarrantyPeriod.LookupId;
                AssetService assetService = new AssetService(User.Identity.Name);
                assetService.SaveAsset(asset);
            }


            return new JsonResult();
        }


    
    }
}