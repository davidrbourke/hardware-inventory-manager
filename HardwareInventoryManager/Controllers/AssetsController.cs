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
using HardwareInventoryManager.Services;
using HardwareInventoryManager.Interfaces;

namespace HardwareInventoryManager.Controllers
{
    [Authorize]
    public class AssetsController : AppController, ITenant
    {
        private CustomApplicationDbContext db = new CustomApplicationDbContext();

        private int tenantId;
        
        // GET: Assets
        public ActionResult Index()
        {
            tenantId = LoadTenant();
            var tenants = db.Assets.Include(a => a.Tenant).Include(a => a.AssetMake).Include(a => a.Category).Include(a => a.WarrantyPeriod).Where(t => t.TenantId == tenantId);
            return View(tenants.ToList());
        }

        // GET: Assets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }

        // GET: Assets/Create
        public ActionResult Create()
        {
            ViewBag.TenantOrganisationId = new SelectList(db.Organisations, "OrganisationId", "Name");
            ViewBag.AssetMakeId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Make.ToString()), "LookupId", "Description");
            ViewBag.CategoryId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Category.ToString()), "LookupId", "Description");
            ViewBag.WarrantyPeriodId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.WarrantyPeriod.ToString()), "LookupId", "Description");
            return View();
        }

        // POST: Assets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenantId,TenantOrganisationId,CreatedDate,UpdatedDate,AssetId,AssetMakeId,Model,SerialNumber,PurchaseDate,WarrantyPeriodId,ObsolescenseDate,PricePaid,CategoryId,LocationDescription")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                tenantId = LoadTenant();
                asset.TenantId = tenantId;
                db.Assets.Add(asset);
                db.SaveChanges();
                Alert(EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);
                return RedirectToAction("Index");
            }
            Alert(EnumHelper.Alerts.Error, HIResources.Strings.Change_Error);
            ViewBag.TenantOrganisationId = new SelectList(db.Organisations, "OrganisationId", "Name", asset.TenantId);
            ViewBag.AssetMakeId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Make.ToString()), "LookupId", "Description");
            ViewBag.CategoryId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Category.ToString()), "LookupId", "Description");
            ViewBag.WarrantyPeriodId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.WarrantyPeriod.ToString()), "LookupId", "Description");
            return View(asset);
        }

        // GET: Assets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenantOrganisationId = new SelectList(db.Organisations, "OrganisationId", "Name", asset.TenantId);
            ViewBag.AssetMakeId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Make.ToString()), "LookupId", "Description");
            ViewBag.CategoryId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Category.ToString()), "LookupId", "Description");
            ViewBag.WarrantyPeriodId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.WarrantyPeriod.ToString()), "LookupId", "Description");
            return View(asset);
        }

        // POST: Assets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TenantId,TenantOrganisationId,CreatedDate,UpdatedDate,AssetId,AssetMakeId,Model,SerialNumber,PurchaseDate,WarrantyPeriodId,ObsolescenseDate,PricePaid,CategoryId,LocationDescription")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asset).State = EntityState.Modified;
                db.SaveChanges();
                Alert(EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);
                return RedirectToAction("Index");
            }
            ViewBag.TenantOrganisationId = new SelectList(db.Organisations, "OrganisationId", "Name", asset.TenantId);
            ViewBag.AssetMakeId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Make.ToString()), "LookupId", "Description");
            ViewBag.CategoryId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.Category.ToString()), "LookupId", "Description");
            ViewBag.WarrantyPeriodId = new SelectList(db.Lookups.Where(l => l.Type.Description == EnumHelper.LookupTypes.WarrantyPeriod.ToString()), "LookupId", "Description");
            Alert(EnumHelper.Alerts.Error, HIResources.Strings.Change_Error);
            return View(asset);
        }

        // GET: Assets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }

        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asset asset = db.Assets.Find(id);
            db.Assets.Remove(asset);
            db.SaveChanges();
            Alert(EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public int LoadTenant()
        {
            return TenantHelper.LoadTenant(HttpContext.User.Identity.Name);
        }
    }
}
