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


            int len = (int)FileUpload.InputStream.Length;
            byte[] byteInput = reader.ReadBytes(len);

            string inp = System.Text.Encoding.UTF8.GetString(byteInput);

            return new JsonResult();
        }
    }
}