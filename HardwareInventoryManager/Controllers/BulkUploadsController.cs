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
            JsonResult res = new JsonResult();
            res.Data = assets;
            return res;
        }
    
    }
}