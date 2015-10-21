using AutoMapper;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Services.Reporting;
using HardwareInventoryManager.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardwareInventoryManager.Controllers
{
    public class ReportsController : Controller
    {

        private ReportingService _reportingService;

        public ReportingService ReportingService
        {
            get
            {
                if (_reportingService == null)
                {
                    _reportingService = new ReportingService(User.Identity.Name);
                }
                return _reportingService;
            }
            set 
            { 
                _reportingService = value; 
            }
        }

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExpiredWarrantyReport()
        {
            Mapper.CreateMap<Asset, AssetViewModel>();
            Mapper.CreateMap<AssetDetail, AssetDetailViewModel>();
            var l = Mapper.Map<IList<Asset>, IList<AssetViewModel>>(ReportingService.ExpiredWarrantyReport().ToList());
            JObject o = JObject.FromObject(
                new
                {
                    Table = l
                });
            AssetIndexViewModel vm = new AssetIndexViewModel
            {
                AssetListJson = o
            };
            vm.ReportDisplayName = HIResources.Strings.Report_ExpiredWarranty;
            vm.ReportDescription = HIResources.Strings.Report_Desc_ExpiredWarranty;


            IList<Header> header = new List<Header>();
            header.Add(new Header { data = "Model" });
            header.Add(new Header { data = "SerialNumber" });
            header.Add(new Header { data = "AssetMake.Description" });
            header.Add(new Header { data = "PurchaseDateFormatted" });
            header.Add(new Header { data = "WarrantyExpiryDate" });
            header.Add(new Header { data = "LocationDescription" });

            vm.Headers = header;
            JObject headerJson = JObject.FromObject(
                new 
                {
                    Header = header
                });
            vm.ReportHeaders = headerJson;
            return View("Report", vm);
        }

        public ActionResult PastObsoleteDateReport()
        {
            Mapper.CreateMap<Asset, AssetViewModel>();
            Mapper.CreateMap<AssetDetail, AssetDetailViewModel>();
            var l = Mapper.Map<IList<Asset>, IList<AssetViewModel>>(ReportingService.PastObsoleteDateReport().ToList());
            JObject o = JObject.FromObject(
                new
                {
                    Table = l
                });
            AssetIndexViewModel vm = new AssetIndexViewModel
            {
                AssetListJson = o
            };
            vm.ReportDisplayName = HIResources.Strings.Report_PastObsoleteDate;
            vm.ReportDescription = HIResources.Strings.Report_Desc_PastObsoleteDate;

            IList<Header> header = new List<Header>();
            header.Add(new Header { data = "Model" });
            header.Add(new Header { data = "SerialNumber" });
            header.Add(new Header { data = "AssetMake.Description" });
            header.Add(new Header { data = "PurchaseDateFormatted" });
            header.Add(new Header { data = "ObsolescenceDateFormatted" });
            header.Add(new Header { data = "LocationDescription" });
            vm.Headers = header;
            JObject headerJson = JObject.FromObject(
                new
                {
                    Header = header
                });
            vm.ReportHeaders = headerJson;
            return View("Report", vm);
        }
    }

    public class Header
    {
        public string data { get; set; }
    }
}