using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadFilter
    {
        public long LeadId { get; internal set; }
        public string Email { get; internal set; }
        public long AffiliateId { get; internal set; }
        public long AffiliateChannelId { get; internal set; }
        public long BuyerId { get; internal set; }
        public long BuyerChannelId { get; internal set; }
        public long CampaignId { get; internal set; }
        public string AffiliateChannelSubId { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string ZipCode { get; internal set; }
        public decimal BuyerPrice { get; internal set; }
        public short ErrorType { get; internal set; }
        public short ReportType { get; internal set; }
        public short Validator { get; internal set; }
        public string State { get; internal set; }
        public short Status { get; internal set; }
        public string IP { get; set; }
        public int Page { get; set; }
        public int PageSize { get; internal set; }
        public DateTime DateTo { get; internal set; }
        public DateTime DateFrom { get; internal set; }
        public string Notes { get; internal set; }
        public decimal MinPrice { get; internal set; }
    }
}