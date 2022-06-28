using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Membership.Register
{
    public class ActivationModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}