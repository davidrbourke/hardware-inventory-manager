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
using HardwareInventoryManager.HIResources;
using HardwareInventoryManager.Filters;

namespace HardwareInventoryManager.Controllers
{
    [CustomAuthorize]
    public class LookupTypesController : AppController
    {
        private CustomApplicationDbContext db = new CustomApplicationDbContext();

        // GET: LookupTypes
        public ActionResult Index()
        {
            return View(db.LookupTypes.ToList());
        }

        // GET: LookupTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LookupType lookupType = db.LookupTypes.Find(id);
            if (lookupType == null)
            {
                return HttpNotFound();
            }
            return View(lookupType);
        }

        // GET: LookupTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LookupTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LookupTypeId,Description,CreatedDate,UpdatedDate")] LookupType lookupType)
        {
            if (ModelState.IsValid)
            {
                db.LookupTypes.Add(lookupType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lookupType);
        }

        // GET: LookupTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LookupType lookupType = db.LookupTypes.Find(id);
            if (lookupType == null)
            {
                return HttpNotFound();
            }
            return View(lookupType);
        }

        // POST: LookupTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LookupTypeId,Description,CreatedDate,UpdatedDate")] LookupType lookupType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lookupType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lookupType);
        }

        // GET: LookupTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LookupType lookupType = db.LookupTypes.Find(id);
            if (lookupType == null)
            {
                return HttpNotFound();
            }
            return View(lookupType);
        }

        // POST: LookupTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                LookupType lookupType = db.LookupTypes.Find(id);
                db.LookupTypes.Remove(lookupType);
                db.SaveChanges();
                Alert(Services.EnumHelper.Alerts.Success, Strings.Change_Success);
            }
            catch(System.Data.Entity.Infrastructure.DbUpdateException)
            {
                Alert(Services.EnumHelper.Alerts.Error, Strings.Delete_Lookup_Type_Error);
            }
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
