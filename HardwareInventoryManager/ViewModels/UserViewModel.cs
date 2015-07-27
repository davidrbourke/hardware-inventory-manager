using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name="Email Address")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Lockout Enabled")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Access Failed Count")]
        public int AccessFailedCount { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public ICollection<Tenant> UserTenants { get; set; }
    }

    public class UserListViewModel
    {
        public UserListViewModel()
        {

            IList<UserAction> actions = new List<UserAction>
            {
                new UserAction
                {
                    Id = 1,
                    Value = "Select Action"
                },
                new UserAction
                {
                    Id = 2,
                    Value = "Reset Password"
                },
                new UserAction
                {
                    Id = 3,
                    Value = "Lock User Account"
                }
            };

            ActionsList = new System.Web.Mvc.SelectList(actions, "Id", "Value");
        }

        public ICollection<UserViewModel> Users { get; set; }

        public System.Web.Mvc.SelectList ActionsList { get; set; }

        public int SelectionAction { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserAction
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}