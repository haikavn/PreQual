using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Lead
{
    public class BuyersChannelViewModel
    {
        public long BuyersChannelId { get; set; }
        public string BuyersChannelName { get; set; }
        public string BuyerName { get; set; }
        public long BuyerId { get; set; }
        public string Campaign { get; set; }
        public long CampaignId { get; set; }
        public ActivityStatuses Status { get; set; }

        public BuyerChannelType BuyersChannelType { get; set; }
    }
}