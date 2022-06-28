using Adrack.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Lead
{
    [Tracked]

    public class PingTree : BaseEntity
    {
        [Tracked]

        public string Name { get; set; }

        [Tracked]

        public int Quantity { get; set; }

        [Tracked(DisplayName = "Campaign", TableName = "Campaign")]
        public long CampaignId { get; set; }
    }
}
