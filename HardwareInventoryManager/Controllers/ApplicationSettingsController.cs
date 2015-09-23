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
using HardwareInventoryManager.Helpers.User;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Helpers.Account;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Web.Security;

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
                if (_applicationSettingsService == null)
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
            return View(db.ApplicationSettings.Include(x => x.AppSetting)
                .Where(x => x.ScopeType == Helpers.EnumHelper.AppSettingScopeType.Application)
                .ToList());
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

        // GET: ApplicationSettings
        [HttpGet]
        public ActionResult IndexUserSettings()
        {
            IAccountProvider accountProvider = new AspNetAccountProvider(
                HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(),
                HttpContext.GetOwinContext().Authentication);

            UserService userService = new UserService(
                new CustomApplicationDbContext(),
                new UserRepository(User.Identity.Name, accountProvider));

            ApplicationUser user = userService.GetUserByEmail(User.Identity.Name);

            return View(db.ApplicationSettings.Include(x => x.AppSetting)
                .Where(x => x.ScopeType == Helpers.EnumHelper.AppSettingScopeType.User &&
                        x.UserId == user.Id).ToList());
        }

        // POST: ApplicationSettings
        [HttpPost]
        public ActionResult IndexUserSettings(List<ApplicationSetting> settings)
        {
            if (ModelState.IsValid)
            {
                ApplicationSettingsService.UpdateMultipleSettings(settings);
                Alert(Helpers.EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);
                return RedirectToAction("IndexUserSettings");
            }
            Alert(Helpers.EnumHelper.Alerts.Error, HIResources.Strings.Change_Error);
            return View(settings);
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
