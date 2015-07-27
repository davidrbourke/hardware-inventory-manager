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
using HardwareInventoryManager.HIResources;
using HardwareInventoryManager.Filters;

namespace HardwareInventoryManager.Controllers
{
    [CustomAuthorize]
    public class LookupsController : AppController
    {
        private CustomApplicationDbContext db = new CustomApplicationDbContext();

        // GET: Lookups
        public ActionResult Index()
        {
            var lookups = db.Lookups.Include(l => l.Type);
            return View(lookups.ToList());
        }

        // GET: Lookups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lookup lookup = db.Lookups.Include(u => u.Type).First(l => l.LookupId == id);
            if (lookup == null)
            {
                return HttpNotFound();
            }
            return View(lookup);
        }

        // GET: Lookups/Create
        public ActionResult Create()
        {
            ViewBag.LookupTypeId = new SelectList(db.LookupTypes, "LookupTypeId", "Description");
            return View();
        }

        // POST: Lookups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LookupId,Description,LookupTypeId,CreatedDate,UpdatedDate")] Lookup lookup)
        {
            if (ModelState.IsValid)
            {
                db.Lookups.Add(lookup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LookupTypeId = new SelectList(db.LookupTypes, "LookupTypeId", "Description", lookup.LookupTypeId);
            return View(lookup);
        }

        // GET: Lookups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lookup lookup = db.Lookups.Find(id);
            if (lookup == null)
            {
                return HttpNotFound();
            }
            ViewBag.LookupTypeId = new SelectList(db.LookupTypes, "LookupTypeId", "Description", lookup.LookupTypeId);
            return View(lookup);
        }

        // POST: Lookups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LookupId,Description,LookupTypeId,CreatedDate,UpdatedDate")] Lookup lookup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lookup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LookupTypeId = new SelectList(db.LookupTypes, "LookupTypeId", "Description", lookup.LookupTypeId);
            return View(lookup);
        }

        // GET: Lookups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lookup lookup = db.Lookups.Include(u => u.Type).First(l => l.LookupId == id);
            if (lookup == null)
            {
                return HttpNotFound();
            }
            return View(lookup);
        }

        // POST: Lookups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Lookup lookup = db.Lookups.Find(id);
                db.Lookups.Remove(lookup);
                db.SaveChanges();
                Alert(EnumHelper.Alerts.Success, Strings.Change_Success);
                return RedirectToAction("Index");
            }
            catch(System.Data.Entity.Infrastructure.DbUpdateException)
            {
                Alert(EnumHelper.Alerts.Error, Strings.Delete_Lookup_Error);
                return RedirectToAction("Index");
            }
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
