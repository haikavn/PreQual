using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadResponseModel : IBaseOutModel
    {
        public IList<AffiliateResponse> AffiliateResponses { get; internal set; }
        public string AffiliateResponseMessage { get; internal set; }
        public string ReceivedData { get; internal set; }
        public LeadMainContent Lead { get; internal set; }
        public DateTime LeadCreated { get; internal set; }
        public string CampaignName { get; internal set; }
        public string AffiliateName { get; internal set; }
        public string AffiliateChannelName { get; internal set; }
        public string StatusValue { get; internal set; }
        public string Status { get; internal set; }
        public IList<XmlNode> Nodes { get; internal set; }
        public IList<string> AllowedNodes { get; internal set; }
        public IList<LeadResponse> LeadResponseList { get; internal set; }
        public IList<LeadContentDublicate> LeadDuplicateList { get; internal set; }
        public string RequestedAmountCount { get; internal set; }
        public string NetMonthlyIncomeCount { get; internal set; }
        public string PayFrequencyCount { get; internal set; }
        public string DirectDepositCount { get; internal set; }
        public string EmailCount { get; internal set; }
        public string HomePhoneCount { get; internal set; }
        public string IpCount { get; internal set; }
        public LeadGeoData GeoData { get; internal set; }
        public LeadSensitiveData SensitiveData { get; internal set; }
        public RedirectUrl RedirectUrl { get; internal set; }
        public string[] LeadEmailFields { get; internal set; }
    }
}