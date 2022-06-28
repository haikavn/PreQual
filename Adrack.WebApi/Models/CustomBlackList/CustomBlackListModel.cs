using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.Campaigns;

namespace Adrack.WebApi.Models.CustomBlackList
{
    public class CustomBlackListModel
    {
        public long Id { get; set; }
        public long ChannelId { get; set; }
        public short ChannelType { get; set; }
        public List<string> Values { get; set; }
        public long? TemplateFieldId { get; set; }
        public CampaignFieldModel  TemplateField { get; set; } 
    }
}