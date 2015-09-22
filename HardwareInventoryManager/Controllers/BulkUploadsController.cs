using AutoMapper;
using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Helpers.Assets;
using HardwareInventoryManager.Helpers.Import;
using HardwareInventoryManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Linq;
using HardwareInventoryManager.Filters;
namespace HardwareInventoryManager.Controllers
{
    [CustomAuthorize]
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
            BulkUploadViewModel response = importService.PrepareImport(FileUpload);
            string batchId = importService.BatchId;

            IEnumerable<TenantViewModel> listOfTenants = GetTenants();
            response.BatchId = batchId;
            response.Tenants = listOfTenants;
                
            Utilities.JsonCamelCaseResult result =
                new Utilities.JsonCamelCaseResult(response, JsonRequestBehavior.AllowGet);
            return result;
        }

        [HttpPost]
        public ActionResult ConfirmImport(BulkUploadViewModel batch)
        {
            ImportService importService = new ImportService(User.Identity.Name);
            int countOfAssetsAdded = importService.ProcessCommit(batch.BatchId, batch.SelectedTenant);

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


        private IEnumerable<TenantViewModel> GetTenants()
        {
            CustomApplicationDbContext context = new CustomApplicationDbContext();
            IQueryable<Tenant> tenants = context.Tenants.Where(t => t.Users.Where(u => u.UserName == User.Identity.Name).Any());
            Mapper.CreateMap<Tenant, TenantViewModel>();
            var listOfTenantViewModel = Mapper.Map<IEnumerable<Tenant>, IEnumerable<TenantViewModel>>(tenants.ToList());
            return listOfTenantViewModel;
        }

        [HttpGet]
        public ActionResult RefreshReview(int id)
        {
            ImportService importService = new ImportService(User.Identity.Name);
            BulkUploadViewModel response = importService.PrepareImport(id);
           
            IEnumerable<TenantViewModel> listOfTenants = GetTenants();
            response.BatchId = id.ToString();
            response.Tenants = listOfTenants;

            Utilities.JsonCamelCaseResult result =
                new Utilities.JsonCamelCaseResult(response, JsonRequestBehavior.AllowGet);
            return result;
        }
    }
}