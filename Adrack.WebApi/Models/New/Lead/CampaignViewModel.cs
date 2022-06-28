using System;

namespace Adrack.WebApi.Models.New.Lead
{
    public class CampaignViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public short Status { get; set; }
        public short Visibility { get; set; }
        public string Description { get; set; }
        public short CampaignType { get; set; }
        public short PriceFormat { get; set; }
        public string XmlTemplate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long VerticalId { get; set; }
        public bool IsTemplate { get; set; }
        public string CampaignKey { get; set; }
        public decimal NetworkTargetRevenue { get; set; }
        public decimal NetworkMinimumRevenue { get; set; }
        public bool? Deleted { get; set; }
        public Guid? HtmlFormId { get; set; }
        public short? PingTreeCycle { get; set; }
        public bool? PrioritizedEnabled { get; set; }
    }
}