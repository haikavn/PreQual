using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelExtendedModel : AffiliateChannelModel
    {
        public AffiliateChannelExtendedModel()
        {
            
        }
        public string CampaignName { get; internal set; }
        public string AffiliateName { get; internal set; }
        public string ChannelId { get; internal set; }
    }
}