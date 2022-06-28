using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Employee
{
    public class EmployeeAdvancedModel
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string JobTitle { get; set; }
        public long? RoleId { get; set; }
        public string LogoPath { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public List<IdNameModel> Campaigns { get; set; }
        public List<IdNameModel> Buyers { get; set; }
        public List<IdNameModel> BuyerChannels { get; set; }
        public List<IdNameModel> Affiliates { get; set; }
        public List<IdNameModel> AffiliateChannels { get; set; }

        public EmployeeAdvancedModel()
        {
            Campaigns = new List<IdNameModel>();
            Buyers = new List<IdNameModel>();
            BuyerChannels = new List<IdNameModel>();
            Affiliates = new List<IdNameModel>();
            AffiliateChannels = new List<IdNameModel>();
        }
    }
}