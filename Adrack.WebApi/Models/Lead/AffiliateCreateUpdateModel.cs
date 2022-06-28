using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Interfaces;
using Adrack.WebApi.Models.Users;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateCreateUpdateModel : AffiliateModelBase
    {
        [Required]
        public long CountryId { get; set; }
        [Required]
        public long ManagerId { get; set; }
        [Required]
        public long StateProvinceId { get; set; }
        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string CompanyEmail { get; set; }
        [Required]
        public string CompanyPhone { get; set; }
        [Required]
        public string ZipPostalCode { get; set; }
        //[Required]
        public string BillFrequency { get; set; }
        //[Required]
        public int FrequencyValue { get; set; }
        //[Required]
        public int BillWithin { get; set; }
        //[Required]
        public string Website { get; set; }
        //[Required]
        public string WhiteIp { get; set; }

        public short Status { get; set; }
        [Required]
        public decimal DefaultAffiliatePrice { get; set; }
        [Required]
        public AffiliatePaymentMethods DefaultAffiliatePriceMethod { get; set; }

        public List<AffiliateInvitationModel> Invitations { get; set; }

        public bool CanSendEmail { get; set; } = true;
    }
}