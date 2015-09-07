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
using HardwareInventoryManager.Services.Account;
using HardwareInventoryManager.Services.Messaging;

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
            IAccountProvider accountProvider = new AspNetAccountProvider(
                HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(),
                HttpContext.GetOwinContext().Authentication);

            IUserService userService = UserTypeFactory.GetUserService(new UserServiceUoW(User.Identity.GetUserId(), accountProvider));
            IList<UserViewModel> listOfUsers = Mapper.Map<IList<ApplicationUser>, IList<UserViewModel>>(
                userService.GetUsers().ToList());
            UserListViewModel userListViewModel = new UserListViewModel
            {
                Users = listOfUsers
            };
            userService.UpdateUserTenants();
            return View(userListViewModel);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IUserService userService = GetUserService();
            ApplicationUser applicationUser = userService.GetUserById(User.Identity.GetUserId());
                
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
            userViewModel.IsCurrentUserRoleAdmin = User.IsInRole("Admin");
            if (User.IsInRole("Admin"))
            {
                PopulateUserDropDowns(userViewModel);
                return View(userViewModel);
            } else
            {
                PopulateFixedValues(userViewModel);
                return View("", userViewModel);
            }
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,PhoneNumber,Role,LockoutEnabled,TenantId")] UserViewModel userViewModel)
        {
            string errors = string.Empty;
            if (ModelState.IsValid)
            {
                if (!User.IsInRole(EnumHelper.Roles.Admin.ToString()) && userViewModel.Role.Equals(EnumHelper.Roles.Admin.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("Illegal privilege escalation");
                }

                IUserService userService = GetUserService();
                ApplicationUser applicationUser = new ApplicationUser
                {
                    Email = userViewModel.Email,
                    PhoneNumber = userViewModel.PhoneNumber,
                    UserName = userViewModel.Email,
                    LockoutEnabled = userViewModel.LockoutEnabled,
                    ForcePasswordReset = true,
                    UserTenants = new List<Tenant>
                    {
                        new Tenant
                        {
                            TenantId = int.Parse(userViewModel.TenantId)
                        }
                    },
                    TemporaryRole = userViewModel.Role
                };
                applicationUser = userService.CreateUser(applicationUser);
                
                errors = userService.Errors == null ? string.Empty : userService.Errors.ToString();
                if(!string.IsNullOrWhiteSpace(applicationUser.Id))
                {
                    Alert(EnumHelper.Alerts.Success, HIResources.Strings.Change_Success);
                    return RedirectToAction("Index");
                }
            }
            Alert(EnumHelper.Alerts.Error, string.Format("{0}, {1}",
                HIResources.Strings.Change_Error,
                errors));
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
            IUserService userService = GetUserService();
            ApplicationUser applicationUser = userService.GetUserById(User.Identity.GetUserId());

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
                IUserService userService = GetUserService();
                Mapper.CreateMap<UserViewModel, ApplicationUser>();
                ApplicationUser editUser = Mapper.Map<UserViewModel, ApplicationUser>(userViewModel);
                editUser = userService.EditUser(editUser); ;
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
            string[] excludedRoles = {};
            userViewModel.RoleSelectList = userViewModel.RoleSelectList = BuildRolesSelectList(excludedRoles); 

            // Tenants
            IRepository<Tenant> tenantRepository = new RepositoryWithoutTenant<Tenant>();
            IQueryable<Tenant> tenants = tenantRepository.GetAll();
            SelectList tenantSelectList = new SelectList(tenants, "TenantId", "Name");
            userViewModel.TenantSelectList = tenantSelectList;
        }

        private void PopulateFixedValues(UserViewModel userSimpleViewModel)
        {
            IUserService userService = GetUserService();
            ITenantUtility tenantUtility = new TenantUtility();
            int tenantId = tenantUtility.GetTenantIdFromEmail(User.Identity.Name);
            ApplicationUser user = userService.GetUserByEmail(User.Identity.Name);

            userSimpleViewModel.TenantObj = user.UserTenants.FirstOrDefault();
            userSimpleViewModel.TenantId = userSimpleViewModel.TenantObj.TenantId.ToString();

            string[] excludedRoles = { "Admin" };
            userSimpleViewModel.RoleSelectList = BuildRolesSelectList(excludedRoles);
        }


        public SelectList BuildRolesSelectList(string[] excludedRoles)
        {
            // Roles
            IList<EnumHelper.Roles> roles = Enum.GetValues(typeof(EnumHelper.Roles)).Cast<EnumHelper.Roles>().ToList();
            IList<SelectListItemObject> roleObjects = new List<SelectListItemObject>();
            foreach (var role in roles)
            {
                if (excludedRoles.Contains(role.ToString()))
                    continue;
                roleObjects.Add(
                    new SelectListItemObject
                    {
                        Id = role.ToString(),
                        Value = role.ToString()
                    });
            }
            SelectList roleSelectList = new SelectList(roleObjects, "Id", "Value");
            return roleSelectList;
        }


        /// <summary>
        /// Gets the user service
        /// </summary>
        /// <returns></returns>
        private IUserService GetUserService()
        {
            IAccountProvider accountProvider = new AspNetAccountProvider(
               HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(),
               HttpContext.GetOwinContext().Authentication);

            IUserService userService = UserTypeFactory.GetUserService(new UserServiceUoW(User.Identity.GetUserId(), accountProvider));
            return userService;
        }
    }
}
