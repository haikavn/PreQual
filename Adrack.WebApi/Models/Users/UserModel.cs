using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Users
{
    public class UserModel
    {
        public long UserId { get; set; } // INFO: Maybe this is a temporary property here
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string RoleName { get; set; }
        public DateTime LastLoginDate { get; set; }

        [Required(ErrorMessage = "Old password is required")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("NewPassword", ErrorMessage ="New Password and Confirm Password fields do not match")]
        public string ConfirmPassword { get; set; }

        public string ProfilePictureURL { get; set; }
    }
}