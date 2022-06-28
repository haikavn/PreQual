using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.Employee
{
    public class EmployeeModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public long RoleId { get; set; }
        public long PlanId { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public UserTypes UserType { get; set; }
        public List<long> Campaigns { get; set; }
        public List<long> Buyers { get; set; }
        public List<long> BuyerChannels { get; set; }
        public List<long> Affiliates { get; set; }
        public List<long> AffiliateChannels { get; set; }
    }
}