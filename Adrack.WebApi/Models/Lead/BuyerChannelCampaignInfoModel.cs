using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.Lead
{
    public class BuyerChannelCampaignInfoModel
    {
        public Campaign Campaign { get; internal set; }
        public Core.Domain.Lead.BuyerChannel BuyerChannel { get; internal set; }
        public Buyer Buyer { get; internal set; }
        public string XmlTemplate { get; internal set; }
        public string BaseUrl { get; internal set; }
        public BuyerChannelModel BuyerChannelModel { get; internal set; }
    }
}