using System.Collections.Generic;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelTreeItem
    {
        public string Title { get; set; }
        public bool Folder { get; set; }
        public bool Expanded { get; set; }
        public string TemplateField { get; set; }
        public long CampaignTemplateId { get; set; }
        public string DefaultValue { get; set; }
        public short MinPriceOption { get; set; }
        public int MinPriceOptionValue { get; set; }
        public decimal MinRevenue { get; set; }
        public List<AffiliateChannelTreeItem> Children { get; set; }

        public AffiliateChannelTreeItem()
        {
            Children = new List<AffiliateChannelTreeItem>();
        }
    }
}