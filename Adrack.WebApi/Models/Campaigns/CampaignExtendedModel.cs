using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Campaigns
{
    public class CampaignExtendedModel : BaseModel
    {
        public CampaignModel CampaignModel { get; set; }
        public string JsonData { get; set; }
        public string XmlData { get; set; }
        public string IsChanged { get; set; }
        public string PingTree { get; set; }
    }
}