using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Click
{

    public class ClickPostBackUrl : BaseEntity
    {
        public long ClickChannelId { get; set; }

        public string PostingUrl { get; set; }

        public string PostingParams { get; set; }
    }
}


