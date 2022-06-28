using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Adrack.Web.Framework;

namespace Adrack.WebApi.Models.Membership.Login
{
    public class SignInModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}