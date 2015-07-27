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
using HardwareInventoryManager.Filters;
using AutoMapper;
using HardwareInventoryManager.ViewModels;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Services.User;
using HardwareInventoryManager.Services;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace HardwareInventoryManager.Controllers
{
    [CustomAuthorize]
    public class UsersController : AppController
    {
        private CustomApplicationDbContext db = new CustomApplicationDbContext();
      
        // GET: Users
        public ActionResult Index()
        {
            Mapper.CreateMap<ApplicationUser, UserViewModel>();
            int tenantId = GetTenantContextId();

            var factory = UserTypeFactory.GetUserService(GetUserRole(), db);
            var l = Mapper.Map<IList<ApplicationUser>, IList<UserViewModel>>(
                factory.GetUsers(tenantId).ToList());

            UserListViewModel u = new UserListViewModel { 
                Users = l
            };

            return View(u);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var factory = UserTypeFactory.GetUserService(GetUserRole(), db);
            ApplicationUser applicationUser = factory.GetUser(GetTenantContextId(), id);
                
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            Mapper.CreateMap<ApplicationUser, UserViewModel>();
            UserViewModel detailUserViewModel = Mapper.Map<ApplicationUser, UserViewModel>(applicationUser);
            return View(detailUserViewModel);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var factory = UserTypeFactory.GetUserService(GetUserRole(), db);
            ApplicationUser applicationUser = factory.GetUser(GetTenantContextId(), id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            Mapper.CreateMap<ApplicationUser, UserViewModel>();
            UserViewModel detailUserViewModel = Mapper.Map<ApplicationUser, UserViewModel>(applicationUser);
            return View(detailUserViewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed,LockoutEnabled,UserName")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {

                var factory = UserTypeFactory.GetUserService(GetUserRole(), db);
                Mapper.CreateMap<UserViewModel, ApplicationUser>();
                ApplicationUser editUser = Mapper.Map<UserViewModel, ApplicationUser>(userViewModel);
                editUser = factory.EditUser(GetTenantContextId(), editUser);
                Alert(EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);
                return RedirectToAction("Index");
            }
            Alert(EnumHelper.Alerts.Error, HIResources.Strings.Change_Error);
            return View(userViewModel);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
