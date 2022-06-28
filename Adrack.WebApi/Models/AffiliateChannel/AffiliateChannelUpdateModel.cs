namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelUpdateModel
    {
        public long Id { get; set; }
        public long CampaignId { get; set; }
        public long AffiliateId { get; set; }
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
    }
}