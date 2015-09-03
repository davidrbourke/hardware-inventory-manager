using AutoMapper;
using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Services.Assets;
using HardwareInventoryManager.Services.Import;
using HardwareInventoryManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
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

            Mapper.CreateMap<Asset, AssetViewModel>();
            var assetsForView = Mapper.Map<IList<AssetViewModel>>(assets);

            Utilities.JsonCamelCaseResult result =
                new Utilities.JsonCamelCaseResult(
                    new BulkUploadViewModel
                    {
                        BatchId = batchId,
                        Assets = assetsForView
                    },
                    JsonRequestBehavior.AllowGet);
            return result;
        }

        [HttpPost]
        public ActionResult ConfirmImport(BulkUploadViewModel batch)
        {
            ImportService importService = new ImportService(User.Identity.Name);
            int countOfAssetsAdded = importService.ProcessCommit(batch.BatchId);

            Utilities.JsonCamelCaseResult result =
                new Utilities.JsonCamelCaseResult(
                    new BulkUploadViewModel
                    {
                        Success = true,
                        Message = string.Format("{0} assets imported", countOfAssetsAdded)
                    },
                    JsonRequestBehavior.AllowGet);
            return result;
        }

        [HttpGet]
        public ActionResult DownloadCsvTemplate()
        {
            string path = string.Format(@"{0}{1}\{2}",
                (HttpContext.Request).PhysicalApplicationPath,
                @"Files",
                "im_import_template.csv");
            FileStream fileStream = System.IO.File.Open(path, FileMode.Open);
            BinaryReader bin = new BinaryReader(fileStream);
            byte[] bytes = bin.ReadBytes((int)fileStream.Length);
            return File(bytes, "text/csv", "im_import_template.csv");
        }   
    }
}