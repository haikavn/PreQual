using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Users
{
    public class UserProfileByEmailModel
    {
        public long UserId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LogoPath { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
    }
}