using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadFilterModel
    {
        public string Actions { get; internal set; }
        public string Password { get; internal set; }
        public string Page { get; internal set; }
        public string PageSize { get; internal set; }
        public string LeadId { get; internal set; }
        public string DateFrom { get; internal set; }
        public string DateTo { get; internal set; }
        public string Dates { get; internal set; }
        public string Status { get; internal set; }
        public string State { get; internal set; }
        public string IP { get; internal set; }
        public string Email { get; internal set; }
        public string AffiliateId { get; internal set; }
        public string AffiliateIds { get; internal set; }

        public string AffiliateChannelId { get; internal set; }
        public string AffiliateChannelSubId { get; internal set; }
        public string BuyerId { get; internal set; }
        public string BuyerChannelId { get; internal set; }
        public string CampaignId { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string ZipCode { get; internal set; }
        public string BuyerPrice { get; internal set; }
        public string LeadIP { get; internal set; }
        public string ClickIP { get; internal set; }
        public string ErrorType { get; internal set; }
        public string Validator { get; internal set; }
        public string ReportType { get; internal set; }
        public string IsReport { get; internal set; }
        public string Notes { get; internal set; }
        public string MinPrice { get; internal set; }
    }
}