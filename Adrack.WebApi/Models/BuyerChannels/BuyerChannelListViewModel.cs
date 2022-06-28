using System;
using System.Collections.Generic;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Lead;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelListViewModel
    {
        public List<BuyerChannelListItem> BuyerChannels { get; set; } = new List<BuyerChannelListItem>();
        //public long BuyerId { get; set; }
        //public string BuyerName { get; set; }
        //public string BuyerIcon { get; set; }
    }

    public class BuyerChannelListItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long BuyerId { get; set; }
        public string BuyerName { get; set; }
        public string BuyerIcon { get; set; }
        public EntityStatus Status { get; set; }
        public long CampaignId { get; set; }
        public string CampaignName { get; set; }

        public short TypeId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}