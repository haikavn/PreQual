using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.Employee
{
    public class InvitedEmployeeModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string InvitedUserToken { get; set; }
    }
}