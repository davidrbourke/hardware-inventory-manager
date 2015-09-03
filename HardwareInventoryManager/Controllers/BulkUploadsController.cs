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
    }
}