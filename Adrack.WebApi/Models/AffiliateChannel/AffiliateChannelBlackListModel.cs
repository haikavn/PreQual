using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelBlackListModel
    {
        public long FieldId { get; set; }
        public List<string> Values { get; set; }
    }
}