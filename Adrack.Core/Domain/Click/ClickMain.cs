using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Click
{
    public enum ClickTypes : short
    {
        View = 1,
        Click = 2
    }
    public class ClickMain : BaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public long ClickChannelId { get; set; }

        public ClickTypes ClickType { get; set; }

        public string IpAddress { get; set; }

        public decimal ClickPrice { get; set; }
    }
}


