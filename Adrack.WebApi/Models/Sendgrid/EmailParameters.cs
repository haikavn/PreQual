using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Sendgrid
{
    public class EmailParameters
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string  UserName { get; set; }
        public string Password { get; set; }
    }
}