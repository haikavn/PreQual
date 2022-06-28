using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adrack.Web.Framework;

namespace Adrack.WebApi.Models.Membership.Password
{
    public class ForgotPasswordModel
    {
        [AppLocalizedStringDisplayName("Membership.ForgotPassword.Email")]
        public string Email { get; set; }
    }
}