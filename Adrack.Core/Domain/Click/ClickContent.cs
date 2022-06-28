using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Click
{

    public class ClickContent : BaseEntity
    {
        public long ClickChannelId { get; set; }

        public long AffiliateChannelId { get; set; }

        public string ParamName { get; set; }

        public string ParamValue { get; set; }

        public long ClickMainId { get; set; }
    }
}


