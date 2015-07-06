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

namespace HardwareInventoryManager.Controllers
{
    public class AssetsController : Controller
    {
        private CustomApplicationDbContext db = new CustomApplicationDbContext();

        // GET: Assets
        public ActionResult Index()
        {
            var tenants = db.Assets.Include(a => a.TenantOrganisation).Include(a => a.AssetMake).Include(a => a.Category).Include(a => a.WarrantyPeriod);
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
            ViewBag.AssetMakeId = new SelectList(db.Lookups, "LookupId", "Description");
            ViewBag.CategoryId = new SelectList(db.Lookups, "LookupId", "Description");
            ViewBag.WarrantyPeriodId = new SelectList(db.Lookups, "LookupId", "Description");
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
                db.Tenants.Add(asset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TenantOrganisationId = new SelectList(db.Organisations, "OrganisationId", "Name", asset.TenantOrganisationId);
            ViewBag.AssetMakeId = new SelectList(db.Lookups, "LookupId", "Description", asset.AssetMakeId);
            ViewBag.CategoryId = new SelectList(db.Lookups, "LookupId", "Description", asset.CategoryId);
            ViewBag.WarrantyPeriodId = new SelectList(db.Lookups, "LookupId", "Description", asset.WarrantyPeriodId);
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
            ViewBag.TenantOrganisationId = new SelectList(db.Organisations, "OrganisationId", "Name", asset.TenantOrganisationId);
            ViewBag.AssetMakeId = new SelectList(db.Lookups, "LookupId", "Description", asset.AssetMakeId);
            ViewBag.CategoryId = new SelectList(db.Lookups, "LookupId", "Description", asset.CategoryId);
            ViewBag.WarrantyPeriodId = new SelectList(db.Lookups, "LookupId", "Description", asset.WarrantyPeriodId);
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
                return RedirectToAction("Index");
            }
            ViewBag.TenantOrganisationId = new SelectList(db.Organisations, "OrganisationId", "Name", asset.TenantOrganisationId);
            ViewBag.AssetMakeId = new SelectList(db.Lookups, "LookupId", "Description", asset.AssetMakeId);
            ViewBag.CategoryId = new SelectList(db.Lookups, "LookupId", "Description", asset.CategoryId);
            ViewBag.WarrantyPeriodId = new SelectList(db.Lookups, "LookupId", "Description", asset.WarrantyPeriodId);
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
            db.Tenants.Remove(asset);
            db.SaveChanges();
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
    }
}
