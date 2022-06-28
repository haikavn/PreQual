using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelFilterUpdateModel
    {
        public string Value { get; set; }
        public short Condition { get; set; }
        public short ConditionOperator { get; set; }
    }
}