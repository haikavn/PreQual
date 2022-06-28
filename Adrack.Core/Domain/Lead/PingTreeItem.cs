using Adrack.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Lead
{
    [Tracked(EntityName = "PingTree", EntityIdField = "PingTreeId")]

    public class PingTreeItem : BaseEntity
    {
        [Tracked(DisplayName = "BuyerChannel", TableName = "BuyerChannel")]

        public long BuyerChannelId { get; set; }

        [Tracked]

        public int OrderNum { get; set; }

        [Tracked]

        public int GroupNum { get; set; }

        [Tracked]

        public int Percent { get; set; }

        [Tracked]

        public bool IsLocked { get; set; }

        public long PingTreeId { get; set; }

        public EntityStatus Status { get; set; }
    }
}
