using System;

namespace Adrack.Core.Domain.Lead.Reports
{
    public partial class TotalRemaining
    {
        public int TotalLeads { get; set; }

        public int TotalPings { get; set; }

        public decimal TotalBuyerSum { get; set; }

        public decimal TotalBuyerChannelSum { get; set; }

    }
}