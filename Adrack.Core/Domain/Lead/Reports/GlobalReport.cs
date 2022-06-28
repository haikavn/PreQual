using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Lead.Reports
{
    public class GlobalReport
    {
        public DateTime? Created { get; set; } = null;

        public long? CampaignId { get; set; } = null;

        public string CampaignName { get; set; }

        public long? BuyerId { get; set; } = null;

        public string BuyerName { get; set;}

        public long? BuyerChannelId { get; set; } = null;

        public string BuyerChannelName { get; set; }

        public long? AffiliateId { get; set; } = null;

        public string AffiliateName { get; set; }

        public long? AffiliateChannelId { get; set; } = null;

        public string AffiliateChannelName { get; set; }

        public int PostedLeads { get; set; }

        public int SoldLeads { get; set; }

        public int RejectedLeads { get; set; }

        public decimal Revenue { get; set; }

        public decimal Paid { get; set; }

        public decimal Profit { get; set; }
    }
}
