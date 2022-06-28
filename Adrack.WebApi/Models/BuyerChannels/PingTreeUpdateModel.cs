using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class PingTreeUpdateModel
    {
        public long Id { get; set; }

        public int Order { get; set; }

        public int Group { get; set; }
        public float PercentageSplits { get; set; }
        public bool Status { get; set; }

        public bool IsFixed { get; set; }

        public bool AlwaysBuyerPrice { get; set; }
    }
}