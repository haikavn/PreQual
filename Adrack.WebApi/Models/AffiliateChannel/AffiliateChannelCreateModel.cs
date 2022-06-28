using System.Collections.Generic;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelCreateModel
    {
        public long? CampaignId { get; set; }
        public string Name { get; set; }
        public long AffiliateId { get; set; }
        public short DataFormat { get; set; }
        public short Status { get; set; }
        public short? Timeout { get; set; }
        public string Note { get; set; }
        public decimal FixedValue { get; set; }
        public decimal NetworkTargetRevenue { get; set; }
        public decimal NetworkMinimumRevenue { get; set; }
        public bool IsFixed { get; set; }
        public bool IsRevenue { get; set; }
        public bool IsInheritFromAffiliate { get; set; }
        public string AffiliateChannelKey { get; set; }
        public string AffiliateChannelPassword { get; set; }

        public short AffiliatePriceMethod { get; set; }

        public decimal AffiliatePrice { get; set; }

        public string XmlTemplate { get; set; }

        public List<string> Templates { get; set; } = new List<string>();
        public List<AffiliateChannelIntegrationModel> AffiliateChannelFields { get; set; }
        public List<AffiliateChannelBlackListModel> AffiliateChannelBlackLists { get; set; }
        public List<AffiliateChannelFilterCreateModel> AffiliateChannelFilters { get; set; }
    }
}