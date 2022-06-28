using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateModel : AffiliateModelBase
    {
        public long CountryId { get; set; }
        public long ManagerId { get; set; }
        public long StateProvinceId { get; set; }
        public long ParentId { get; set; }
        public long UserRoleId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string ZipPostalCode { get; set; }
        public string BillFrequency { get; set; }
        public int FrequencyValue { get; set; }
        public int BillWithin { get; set; }
        public string Website { get; set; }
        public string WhiteIp { get; set; }
        public short Status { get; set; }
        public List<AffiliateNote> Notes { get; set; }
        public decimal DefaultAffiliatePrice { get; set; }
        public short DefaultAffiliatePriceMethod { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Password { get; set; }
        public string Comment { get; set; }
        public string ContactEmail { get; set; }
        public bool IsNotRegistered { get; set; }
        public string Icon { get; set; }
    }
}