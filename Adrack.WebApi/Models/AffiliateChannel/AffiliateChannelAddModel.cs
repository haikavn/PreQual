using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelAddModel : AffiliateChannelModel
    {
        public string Xml { get; internal set; }
        public string Conditions { get; internal set; }
        public string BlackList { get; internal set; }
        public string JsonValue { get; internal set; }
    }
}