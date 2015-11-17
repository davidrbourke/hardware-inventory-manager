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
using HardwareInventoryManager.Helpers.Assets;
using HardwareInventoryManager.Helpers.User;

namespace HardwareInventoryManager.Controllers
{
    [CustomAuthorize]
    public class AssetItemsController : AppController
    {
        CustomApplicationDbContext db = new CustomApplicationDbContext();

        private AssetService _assetService;
        public AssetService AssetService
        {
            get
            {
                if (_assetService == null)
                {
                    _assetService = new  AssetService(User.Identity.Name);
                }
                return _assetService;
            }
            set
            {
                _assetService = value;
            }
        }

        // GET: Assets
        public ActionResult Index()
        {
            IList<Asset> assets = AssetService.GetAllAssets().ToList();
            Mapper.CreateMap<Asset, AssetViewModel>();
            Mapper.CreateMap<AssetDetail, AssetDetailViewModel>();
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
            Asset asset = AssetService.GetSingleAsset(id.Value);
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
        public ActionResult Create(AssetViewModel createAssetViewModel)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<AssetViewModel, Asset>();
                Mapper.CreateMap<AssetDetailViewModel, AssetDetail>();
                Asset asset = Mapper.Map<AssetViewModel, Asset>(createAssetViewModel);
                AssetService.SaveAsset(asset);
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
            Asset asset = AssetService.GetSingleAsset(id.Value);
            if (asset == null)
            {
                return HttpNotFound();
            }
            Mapper.CreateMap<Asset, AssetViewModel>();
            Mapper.CreateMap<AssetDetail, AssetDetailViewModel>();
            AssetViewModel editAssetViewModel = Mapper.Map<Asset, AssetViewModel>(asset);
            if(editAssetViewModel.NetworkedAssetDetail == null)
            {
                editAssetViewModel.NetworkedAssetDetail = new AssetDetailViewModel();
            }
            PopulateSelectLists(editAssetViewModel);
            return View(editAssetViewModel);
        }

        // POST: Assets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AssetViewModel editAssetViewModel)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<AssetViewModel, Asset>();
                Asset asset = Mapper.DynamicMap<AssetViewModel, Asset>(editAssetViewModel);
                asset.PurchaseDate = editAssetViewModel.PurchaseDate;
                asset.AssetDetailId = asset.NetworkedAssetDetail.AssetDetailId;
                AssetService.SaveAsset(asset);
                Alert(EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);
                return RedirectToAction("Index");
            }
            
            var items = ModelState.ToList().Where(c => c.Value.Errors.Count() > 0);

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
            Asset asset = AssetService.GetSingleAsset(id.Value);
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
            AssetService.Delete(id);
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
            IQueryable<Tenant> userTenants = new TenantUtility().GetUserTenants(User.Identity.Name);
            assetViewModel.TenantOrganisationSelectList = new SelectList(userTenants, "TenantId", "Name", assetViewModel.TenantId);
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
