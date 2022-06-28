using System.Collections.Generic;
using System.Web.Mvc;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Settings
{
    public class SettingModel : BaseModel, IBaseInModel
    {
        public bool IsSaved { get; internal set; }
        public string AppUrl { get; set; }
        public short? DebugMode { get; internal set; }
        public short? SystemOnHold { get; set; }
        public string AutoCacheUrls { get; set; }
        public short AutoCacheMode { get; set; }
        public string AutoCacheModeString { get; set; }
        public short? MinProcessingMode { get; set; }
        public string LeadEmailFields { get; set; }
        public string LeadEmailTo { get; set; }
        public short? LeadEmail { get; set; }
        public string WhiteIp { get; set; }
        public short? DuplicateMonitor { get; set; }
        public short? AllowAffiliateRedirect { get; set; }
        public int ProcessingDelay { get; set; }
        public string ProcessingDelayString { get; set; }
        public int? MaxProcessingLeads { get; set; }
        public int? LoginExpire { get; set; }
        public string AffiliateRedirectUrl { get; set; }
        public string AffiliateXmlField { get; set; }
        public string PostingUrl { get; set; }
        public string CompanyLogoPath { get; set; }
        public string CompanyEmail { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountType { get; set; }
        public string CompanyBank { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string CompanyAddress2 { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyName { get; set; }
        public string PageTitle { get; set; }
        public string ErrorMessage { get; internal set; }
        public short EmailProvider { get; set; }
    }
}