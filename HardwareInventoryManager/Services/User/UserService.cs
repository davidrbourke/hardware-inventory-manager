using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using HardwareInventoryManager.Services.User;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using HardwareInventoryManager.Repository;
using HardwareInventoryManager.Services.Messaging;

namespace HardwareInventoryManager.Services.User
{
    public class UserService : IUserService
    {
        private readonly CustomApplicationDbContext _context;
        private IRepository<ApplicationUser> _userRepository;

        public UserService(CustomApplicationDbContext context, IRepository<ApplicationUser> userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public IQueryable<ApplicationUser> GetUsers()
        {
            return _userRepository.GetAll();
        }

        public ApplicationUser GetUserById(string userId)
        {
            return _userRepository.Single(u => u.Id == userId);
        }

        public ApplicationUser GetUserByEmail(string emailAddress)
        {
            return _userRepository.Single(u => u.Email == emailAddress);
        }

        public ApplicationUser EditUser(ApplicationUser user)
        {
            _userRepository.Edit(user);
            return user;
        }

        public EnumHelper.Roles GetCurrentUserRoleById(string userId)
        {
            var store = new UserStore<ApplicationUser>(_context);
            var manager = new UserManager<ApplicationUser>(store);
            EnumHelper.Roles userRole = EnumHelper.Roles.Viewer;
            if (manager.IsInRole(userId, EnumHelper.Roles.Admin.ToString()))
            {
                return EnumHelper.Roles.Admin;
            }
            else if (manager.IsInRole(userId, EnumHelper.Roles.Author.ToString()))
            {
                return EnumHelper.Roles.Author;
            }
            return userRole;
        }


        public void UpdateUserTenants()
        {
        
        }

        public ApplicationUser CreateUser(ApplicationUser user)
        {
            UtilityHelper utilityHelper = new UtilityHelper();
            string temporaryCode = utilityHelper.GeneratePassword();
            user.TemporaryCode = temporaryCode;
            user = _userRepository.Create(user);

            if (!string.IsNullOrWhiteSpace(user.Id))
            {
                IProcessEmail processEmail = new ProcessEmail(user.UserName);
                processEmail.SendNewAccountSetupEmail(user);
            }
            return user;
        }
    }

}