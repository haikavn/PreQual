using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Employee
{
    public class EmployeeUpdateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string JobTitle { get; set; }
        public long RoleId { get; set; }
        public bool IsActive { get; set; }
        public List<long> Campaigns { get; set; }
        public List<long> Buyers { get; set; }
        public List<long> BuyerChannels { get; set; }
        public List<long> Affiliates { get; set; }
        public List<long> AffiliateChannels { get; set; }
        public long PlanId { get; set; }
    }
}