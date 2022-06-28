using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class AttachedBuyerChannelModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Included { get; set; }
        public BuyerChannelStatuses Status { get; set; }
    }
}