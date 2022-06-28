using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignTemplateModel : BaseModel
    {
        public CampaignModelTreeItem Items { get; internal set; }
        public List<CampaignTemplateFieldModel> Fields { get; internal set; }
        public string XmlData { get; internal set; }
        public decimal NetworkTargetRevenue { get; internal set; }
        public decimal NetworkMinimumRevenue { get; internal set; }
        public short CampaignType { get; internal set; }
    }
}