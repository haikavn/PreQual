using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Campaigns;
using System.Collections.Generic;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class ChannelBuyerFilterModel
    {
        public string BuyerIds { get; set; }

        public string CampaignIds { get; set; }

    }
}