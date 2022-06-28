using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadMainContentModel
    {
        public bool HasPermission { get; internal set; }
        public bool HasNotes { get; internal set; }
        public long Id { get; internal set; }
        public DateTime Created { get; internal set; }
        public string EmailResult { get; internal set; }
        public string RedirectUrl { get; internal set; }
        public short Status { get; internal set; }
        public long AffiliateId { get; internal set; }
        public long AffiliateChannelId { get; internal set; }
        public string AffiliateName { get; internal set; }
        public string AffiliateChannelName { get; internal set; }
        public decimal BuyerPrice { get; internal set; }
        public DateTime? Updated { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public decimal AffiliatePrice { get; internal set; }
        public string IP { get; internal set; }
        public string Zip { get; internal set; }
        public string State { get; internal set; }
        public string StatusString { get; internal set; }
        public long? BuyerId { get; internal set; }
        public string BuyerName { get; internal set; }
        public long? BuyerChannelId { get; internal set; }
        public string BuyerChannelName { get; internal set; }
        public long CampaignId { get; internal set; }
        public string CampaignName { get; internal set; }
        public string Monitor { get; internal set; }
        public double? ProcessingTime { get; internal set; }
        public string LeadNoteString { get; internal set; }
    }
}