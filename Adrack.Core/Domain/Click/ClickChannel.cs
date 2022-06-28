using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Click
{
    public class ClickChannel : BaseEntity
    {
        public string Name { get; set; }
        
        public long AffiliateChannelId { get; set; }

        public decimal ClickPrice { get; set; }

        public string AccessKey { get; set; }

        public string RedirectUrl { get; set; }
    }
}


