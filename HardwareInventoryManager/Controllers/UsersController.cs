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
using HardwareInventoryManager.Helpers.User;
using HardwareInventoryManager.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using HardwareInventoryManager.Helpers.Account;
using HardwareInventoryManager.Services.Messaging;
using HardwareInventoryManager.Services.User;

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
            UserViewModel userViewModel = new UserViewModel();
            PopulateUserDropDowns(userViewModel);

            return View(userViewModel);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,PhoneNumber,Role,LockoutEnabled,TenantId")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                UtilityHelper utilityHelper = new UtilityHelper();

                IRepositoryNoTenant<Tenant> tenantRepository = new RepositoryNoTenant<Tenant>();
                int tenantId = int.Parse(userViewModel.TenantId);
                Tenant tenant = tenantRepository.Single(t => t.TenantId == tenantId);

                ApplicationUser applicationUser = new ApplicationUser
                {
                    Email = userViewModel.Email,
                    PhoneNumber = userViewModel.PhoneNumber,
                    UserName = userViewModel.Email,
                    LockoutEnabled = userViewModel.LockoutEnabled,
                    UserTenants = new List<Tenant>
                    {
                        tenant
                    }
                };
                string temporaryCode = utilityHelper.GeneratePassword();
                IdentityResult createUser = userManager.Create(applicationUser, temporaryCode);

                // Check if they can ad admin
                // find role
                EnumHelper.Roles role = (EnumHelper.Roles)Enum.Parse(typeof(EnumHelper.Roles), userViewModel.Role);
                userManager.AddToRole(applicationUser.Id, role.ToString());
                if(createUser.Succeeded)
                {
                    Alert(EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);

                    IProcessEmail processEmail = new ProcessEmail();
                    processEmail.SendNewAccountSetupEmail(applicationUser,
                        userManager, temporaryCode);

                    return RedirectToAction("Index");
                }
            }
            Alert(EnumHelper.Alerts.Error, HIResources.Strings.Change_Error);
            PopulateUserDropDowns(userViewModel);
            return View(userViewModel);
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

        /// <summary>
        /// Populates the user view model with the required dropdowns
        /// </summary>
        /// <param name="userViewModel"></param>
        private void PopulateUserDropDowns(UserViewModel userViewModel)
        {
            // Roles
            IList<EnumHelper.Roles> roles = Enum.GetValues(typeof(EnumHelper.Roles)).Cast<EnumHelper.Roles>().ToList();
            IList<SelectListItemObject> roleObjects = new List<SelectListItemObject>();
            foreach (var role in roles)
            {
                roleObjects.Add(
                    new SelectListItemObject
                    {
                        Id = role.ToString(),
                        Value = role.ToString()
                    });
            }
            SelectList roleSelectList = new SelectList(roleObjects, "Id", "Value");
            userViewModel.RoleSelectList = roleSelectList;

            // Tenants
            IRepositoryNoTenant<Tenant> tenantRepository = new RepositoryNoTenant<Tenant>();
            IQueryable<Tenant> tenants = tenantRepository.GetAll();
            SelectList tenantSelectList = new SelectList(tenants, "TenantId", "Name");
            userViewModel.TenantSelectList = tenantSelectList;
        }

        /// <summary>
        /// Find the recipients email address from the user record and initiates sending the email
        /// </summary>
        /// <param name="recipientUser"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        private void SendEmail(ApplicationUser recipientUser, string subject, string body)
        {
            IRepository<Email> emailRepository = new Repository<Email>();
            IEmailService emailService = new OfflineEmailService(emailRepository);
            IUserUtility userUtility = new UserUtility();
            ITenantUtility tenantUtility = new TenantUtility();
            IProcessEmail processEmail = new ProcessEmail(emailService, userUtility, tenantUtility);

            // TODO: Get the email of the sender from the database
            string sender = "david@drbtechnology.com";
            emailService.SendEmail(sender, recipientUser.Email, subject, body);
        }
    }
}
