using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Adrack.Web.Framework;

namespace Adrack.WebApi.Models.Membership.Password
{
    public class FirstPasswordConfirmationModel
    {
        public string Email { get; set; }
        public string Token { get; set; }

        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ChangePassword.ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }
        //public bool DisablePasswordChanging { get; set; }
    }
}