using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Membership.Login
{
    public class RemoteLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}