using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadItemModel
    {
        public long LeadId { get; set; }

        public DateTime CreatedAt { get; set; }

        public long CampaignId { get; set; }

        public string CampaignName { get; set; }

        public string IpAddress { get; set; }

        public long AffiliateChannelId { get; set; }

        public string AffiliateChannelName { get; set; }

        public long AffiliateId { get; set; }

        public string AffiliateName { get; set; }

        public decimal BuyerPrice { get; set; }

        public decimal AffiliatePrice { get; set; }

        public string MinPrices { get; set; }

        public LeadResponseStatus LeadStatus { get; set; }

        public string LeadStatusName { get; set; }

        public List<LeadCommonInfoItemModel> Fields { get; set; }
        //public LeadCommonInformationModel LeadCommonInf { get; set; }
        public List<LeadLogModel> LeadLogs { get; set; }
        public List<LeadDuplicateMonitorModel> LeadDuplicateMonitors { get; set; }
        public List<LeadAffiliateResponse> AffiliateResponses { get; set; }
        public List<LeadJourneyModel> LeadJourneys { get; set; }
        public RedirectUrl RedirectUrl { get; set; }

        public long nextLeadId { get; set; }
        public long prevLeadId { get; set; }

        public LeadItemModel()
        {
                //LeadCommonInf = new LeadCommonInformationModel();
                LeadLogs = new List<LeadLogModel>();
                LeadDuplicateMonitors = new List<LeadDuplicateMonitorModel>();
                AffiliateResponses = new List<LeadAffiliateResponse>();
                LeadJourneys = new List<LeadJourneyModel>();
        }
    }
}