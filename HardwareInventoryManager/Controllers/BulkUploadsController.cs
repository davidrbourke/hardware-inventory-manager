using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Services.Assets;
using HardwareInventoryManager.Services.Import;
using System;
using System.Collections.Generic;
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
            ImportService importService = new ImportService(User.Identity.Name);
            IEnumerable<Asset> assets = importService.PrepareImport(FileUpload);
            string batchId = importService.BatchId;
            JsonResult res = new JsonResult();
            res.Data = new Batch
            {
                BatchId = batchId,
                Assets = assets
            };
            return res;
        }

        [HttpPost]
        public ActionResult ConfirmImport(Batch batch)
        {
            ImportService importService = new ImportService(User.Identity.Name);
            int countOfAssetsAdded = importService.ProcessCommit(batch.BatchId);
            JsonResult res = new JsonResult();
            res.Data = new Batch{
                Success = true,
                Message = string.Format("{0} assets imported", countOfAssetsAdded)
            };
            return res;
        }
    }

    public class Batch
    {
        public string BatchId { get; set; }
        public IEnumerable<Asset> Assets { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}