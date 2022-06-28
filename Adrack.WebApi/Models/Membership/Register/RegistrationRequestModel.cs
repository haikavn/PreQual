using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Membership.Register
{
    public class RegistrationRequestModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}