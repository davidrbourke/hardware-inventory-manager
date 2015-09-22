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
using HardwareInventoryManager.Helpers.ApplicationSettings;
using HardwareInventoryManager.Filters;

namespace HardwareInventoryManager.Controllers
{
    [CustomAuthorize]
    public class ApplicationSettingsController : AppController
    {

        private IApplicationSettingsService _applicationSettingsService;
        public IApplicationSettingsService ApplicationSettingsService
        {
            get
            {
                if(_applicationSettingsService == null)
                {
                    _applicationSettingsService = new ApplicationSettingsService(User.Identity.Name) 
                        as IApplicationSettingsService;
                }
                return _applicationSettingsService;
            }
            set
            {
                _applicationSettingsService = value;
            }
        }

        private CustomApplicationDbContext db = new CustomApplicationDbContext();

        // GET: ApplicationSettings
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.ApplicationSettings.ToList());
        }
        
        // POST: ApplicationSettings
        [HttpPost]
        public ActionResult Index(List<ApplicationSetting> settings)
        {
            if (ModelState.IsValid)
            {
                ApplicationSettingsService.UpdateMultipleSettings(settings);
                Alert(Helpers.EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);
                return RedirectToAction("Index");
            }
            Alert(Helpers.EnumHelper.Alerts.Error, HIResources.Strings.Change_Error);
            return View(settings);
        }

        // GET: ApplicationSettings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationSetting applicationSetting = db.ApplicationSettings.Find(id);
            if (applicationSetting == null)
            {
                return HttpNotFound();
            }
            return View(applicationSetting);
        }

        // GET: ApplicationSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApplicationSettingId,Key,Value,DataType,ScopeType,TenantId,CreatedDate,UpdatedDate")] ApplicationSetting applicationSetting)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationSettings.Add(applicationSetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationSetting);
        }

        // GET: ApplicationSettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationSetting applicationSetting = db.ApplicationSettings.Find(id);
            if (applicationSetting == null)
            {
                return HttpNotFound();
            }
            return View(applicationSetting);
        }

        // POST: ApplicationSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationSettingId,Key,Value,DataType,ScopeType,TenantId,CreatedDate,UpdatedDate")] ApplicationSetting applicationSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationSetting);
        }

        // GET: ApplicationSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationSetting applicationSetting = db.ApplicationSettings.Find(id);
            if (applicationSetting == null)
            {
                return HttpNotFound();
            }
            return View(applicationSetting);
        }

        // POST: ApplicationSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationSetting applicationSetting = db.ApplicationSettings.Find(id);
            db.ApplicationSettings.Remove(applicationSetting);
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
