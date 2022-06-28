using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Adrack.Web.Framework;

namespace Adrack.WebApi.Models.Membership.Password
{
    public class ChangePasswordModel
    {
        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.Password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [AppLocalizedStringDisplayName("Membership.Field.ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}