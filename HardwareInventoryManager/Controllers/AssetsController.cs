using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HardwareInventoryManager;
using HardwareInventoryManager.Models;
using HardwareInventoryManager.Helpers;
using HardwareInventoryManager.Filters;
using HardwareInventoryManager.Repository;
using AutoMapper;
using HardwareInventoryManager.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HardwareInventoryManager.Controllers
{
    [CustomAuthorize]
    public class AssetsController : AppController
    {
        CustomApplicationDbContext db = new CustomApplicationDbContext();
        private IRepository<Asset> _assetRepository;
        
        public AssetsController()
        {
            _assetRepository = new Repository<Asset>();
        }

        public AssetsController(IRepository<Asset> assetRepository)
        {
            _assetRepository = assetRepository;
        }

        // GET: Assets
        public ActionResult Index()
        {
            _assetRepository.SetCurrentUser(GetCurrentUser());
            IList<Asset> assets = _assetRepository.GetAll()
                .Include(x => x.AssetMake)
                .Include(x => x.Category)
                .Include(x => x.WarrantyPeriod)
                .ToList();

            Mapper.CreateMap<Asset, AssetViewModel>();
            var l = Mapper.Map<IList<Asset>, IList<AssetViewModel>>(assets);


            JObject o = JObject.FromObject(
                new
                {
                    Table = l
                });

            AssetIndexViewModel vm = new AssetIndexViewModel
            {
                AssetListJson = o
            };


            return View(vm);
        }

        // GET: Assets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int tenantId = GetTenantContextId();
            _assetRepository.SetCurrentUser(GetCurrentUser());
            Asset asset = _assetRepository.Find(x => x.AssetId == id)
                .Include(x => x.AssetMake)
                .Include(x => x.Category)
                .Include(x => x.WarrantyPeriod)
                .FirstOrDefault();

            Mapper.CreateMap<Asset, AssetViewModel>();
            AssetViewModel detailAssetViewModel = Mapper.Map<Asset, AssetViewModel>(asset);
            
            PopulateSelectLists(detailAssetViewModel);
            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(detailAssetViewModel);
        }

        // GET: Assets/Create
        public ActionResult Create()
        {
            AssetViewModel createAssetViewModel = new AssetViewModel();
            PopulateSelectLists(createAssetViewModel);
            return View(createAssetViewModel);
        }

        // POST: Assets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenantId,TenantOrganisationId,CreatedDate,UpdatedDate,AssetId,AssetMakeId,Model,SerialNumber,PurchaseDate,WarrantyPeriodId,ObsolescenseDate,PricePaid,CategoryId,LocationDescription")] AssetViewModel createAssetViewModel)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<AssetViewModel, Asset>();
                Asset asset = Mapper.Map<AssetViewModel, Asset>(createAssetViewModel);
                _assetRepository.SetCurrentUser(GetCurrentUser());
                _assetRepository.Create(asset);
                _assetRepository.Save();
                Alert(EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);
                return RedirectToAction("Index");
            }
            Alert(EnumHelper.Alerts.Error, HIResources.Strings.Change_Error);
            PopulateSelectLists(createAssetViewModel);
            return View(createAssetViewModel);
        }

        // GET: Assets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _assetRepository.SetCurrentUser(GetCurrentUser());
            Asset asset =_assetRepository.Find(x => x.AssetId == id)
                .Include(x => x.AssetMake)
                .Include(x => x.Category)
                .Include(x => x.WarrantyPeriod)
                .First();

            if (asset == null)
            {
                return HttpNotFound();
            }
            Mapper.CreateMap<Asset, AssetViewModel>();
            AssetViewModel editAssetViewModel = Mapper.Map<Asset, AssetViewModel>(asset);
            
            PopulateSelectLists(editAssetViewModel);
            return View(editAssetViewModel);
        }

        // POST: Assets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TenantId,TenantOrganisationId,CreatedDate,UpdatedDate,AssetId,AssetMakeId,Model,SerialNumber,PurchaseDate,WarrantyPeriodId,ObsolescenseDate,PricePaid,CategoryId,LocationDescription")] AssetViewModel editAssetViewModel)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<AssetViewModel, Asset>();
                Asset asset = Mapper.DynamicMap<AssetViewModel, Asset>(editAssetViewModel);
                _assetRepository.SetCurrentUser(GetCurrentUser());
                _assetRepository.Edit(asset);
                _assetRepository.Save();
                Alert(EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);
                return RedirectToAction("Index");
            }
            PopulateSelectLists(editAssetViewModel);
            Alert(EnumHelper.Alerts.Error, HIResources.Strings.Change_Error);
            return View(editAssetViewModel);
        }

        // GET: Assets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _assetRepository.SetCurrentUser(GetCurrentUser());
            int tenantContextId = GetTenantContextId();
            var assets = _assetRepository.Find(x => x.AssetId == id);
            Asset asset =_assetRepository.Find(x => x.AssetId == id)
                .Include(x => x.AssetMake)
                .Include(x => x.Category)
                .Include(x => x.WarrantyPeriod)
                .FirstOrDefault();
            
            Mapper.CreateMap<Asset, AssetViewModel>();
            AssetViewModel deleteAssetViewModel = Mapper.DynamicMap<Asset, AssetViewModel>(asset);
            
            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(deleteAssetViewModel);
        }

        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _assetRepository.SetCurrentUser(GetCurrentUser());
            Asset asset = _assetRepository.Find(x => x.AssetId == id).First();
            _assetRepository.Delete(asset);
            _assetRepository.Save();
            Alert(EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Populate the View Model with the required Select Lists/Dropdowns
        /// </summary>
        /// <param name="assetViewModel"></param>
        /// <returns></returns>
        public void PopulateSelectLists(AssetViewModel assetViewModel)
        {
            assetViewModel.TenantOrganisationSelectList = new SelectList(db.Tenants, "TenantId", "Name", assetViewModel.TenantId);
            assetViewModel.AssetMakeSelectList = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Make.ToString()), "LookupId", "Description", assetViewModel.AssetMakeId);
            assetViewModel.CategorySelectList = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Category.ToString()), "LookupId", "Description", assetViewModel.CategoryId);
            assetViewModel.WarrantyPeriodSelectList = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.WarrantyPeriod.ToString()), "LookupId", "Description", assetViewModel.WarrantyPeriodId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
