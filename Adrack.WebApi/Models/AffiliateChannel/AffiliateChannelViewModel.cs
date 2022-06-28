using System;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelViewModel
    {
        public long Id { get; set; }
        public long? CampaignId { get; set; }
        public string CampaignName { get; set; }
        public long AffiliateId { get; set; }
        public string AffiliateName { get; set; }
        public string Name { get; set; }
        public short Status { get; set; }
        public string XmlTemplate { get; set; }
        public short DataFormat { get; set; }
        public short MinPriceOption { get; set; }
        public decimal MinPriceOptionValue { get; set; }
        public decimal MinRevenue { get; set; }
        public string AffiliateChannelKey { get; set; }
        public bool? Deleted { get; set; }
        public short? AffiliatePriceMethod { get; set; }
        public decimal? AffiliatePrice { get; set; }
        public short? Timeout { get; set; }
        public string Note { get; set; }
        public bool IsIntegrated { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}